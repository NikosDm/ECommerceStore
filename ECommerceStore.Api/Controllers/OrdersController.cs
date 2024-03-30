using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceStore.Api.Data;
using ECommerceStore.Api.DTOs;
using ECommerceStore.Api.Entities;
using ECommerceStore.Api.Entities.OrderAggregate;
using ECommerceStore.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceStore.Api.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly StoreContext _context;

        public OrdersController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetOrdersAsync()
        {
            return await _context.Orders
                .ProjectOrderToOrderDTO()
                .Where(x => x.BuyerId == User.Identity.Name)
                .ToListAsync();
        }

        [HttpGet("{id}", Name = "GetOrderAsync")]
        public async Task<ActionResult<OrderDTO>> GetOrderAsync(int id)
        {
            return await _context.Orders
                .ProjectOrderToOrderDTO()
                .Where(x => x.BuyerId == User.Identity.Name && x.Id == id)
                .FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrder(CreateOrderDTO orderDTO)
        {
            var basket = await _context.Baskets
                .RetrieveBasketWithItems(User.Identity.Name)
                .FirstOrDefaultAsync();

            if (basket is null) return BadRequest(new ProblemDetails { Title = "Could not locate basket" });

            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await _context.Products.FindAsync(item.ProductId);

                var itemOrdered = new ProductItemOrdered
                {
                    ProductId = productItem.Id,
                    Name = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };

                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);
                productItem.QuantityInStock -= item.Quantity;
            }

            var subtotal = items.Sum(item => item.Price * item.Quantity);
            var deliveryFee = subtotal > 100 ? 0 : 5;

            var order = new Order
            {
                OrderItems = items,
                BuyerId = User.Identity.Name,
                ShippingAddress = orderDTO.ShippingAddress,
                PaymentDetails = orderDTO.PaymentDetails,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee
            };

            _context.Orders.Add(order);
            _context.Baskets.Remove(basket);
            
            var user = await _context.Users
                    .Include(a => a.Address)
                    .Include(p => p.PaymentDetails)
                    .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            
            if (orderDTO.SaveAddress) 
            {
                var address = new UserAddress
                {
                    FullName = orderDTO.ShippingAddress.FullName,
                    Address1 = orderDTO.ShippingAddress.Address1,
                    Address2 = orderDTO.ShippingAddress.Address2,
                    State = orderDTO.ShippingAddress.State,
                    City = orderDTO.ShippingAddress.City,
                    Country = orderDTO.ShippingAddress.Country,
                    Zip = orderDTO.ShippingAddress.Zip,
                };
                user.Address = address;
            }

            if (orderDTO.SaveCard) 
            {
                var paymentDetails = new UserPaymentDetails
                {
                    CardNumber = orderDTO.PaymentDetails.CardNumber,
                    ExpDate = orderDTO.PaymentDetails.ExpDate,
                    NameOnCard = orderDTO.PaymentDetails.NameOnCard,
                    Cvv = orderDTO.PaymentDetails.Cvv,
                };
                user.PaymentDetails = paymentDetails;
            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetOrderAsync", new { id = order.Id }, order.Id);

            return BadRequest("Problem creating order");
        }
    }
}