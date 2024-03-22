using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceStore.Api.Data;
using ECommerceStore.Api.DTOs;
using ECommerceStore.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceStore.Api.Controllers
{
    public class BasketController : BaseController
    {
        private readonly StoreContext _context;
        
        public BasketController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet(Name="GetBasket")]
        public async Task<ActionResult<BasketDTO>> GetBasket()
        {
            var basket = await RetrieveBasket();

            if (basket is null) return NotFound();
            return MapBasketToDTO(basket);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDTO>> AddItemToBasket(int productId, int quantity)
        {
            // get basket || create basket
            var basket = await RetrieveBasket();

            if (basket is null) basket = CreateBasket();

            // get product
            var product = await _context.Products.FindAsync(productId);

            if (product is null) return BadRequest(new ProblemDetails { Title ="Product not found" });

            // add item
            basket.AddItem(product, quantity);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetBasket", MapBasketToDTO(basket));

            return BadRequest(new ProblemDetails { Title = "Problem saving item to basket" });
        }

        [HttpDelete]
        public async Task<ActionResult<Basket>> RemoveBasketItem(int productId, int quantity)
        {
            // get basket
            var basket = await RetrieveBasket();

            if (basket is null) return NotFound();

            // remove item or reduce quantity
            basket.RemoveItem(productId, quantity);

            // save changes
            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem removing item from basket" });
        }

        private BasketDTO MapBasketToDTO(Basket basket)
        {
            return new BasketDTO
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                Items = basket.Items.Select(item => new BasketItemDTO
                {
                    ProductId = item.ProductId,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    Description = item.Product.Description,
                    Type = item.Product.Type,
                    Brand = item.Product.Brand,
                    Quantity = item.Quantity
                }).ToList()
            };
        }

        private async Task<Basket> RetrieveBasket()
        {
            return await _context.Baskets
                .Include(x => x.Items)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(x => x.BuyerId == Request.Cookies["buyerId"]);
        }
        
        private Basket CreateBasket()
        {
            var buyerId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30) };
            Response.Cookies.Append("buyerId", buyerId, cookieOptions);

            var basket = new Basket { BuyerId = buyerId };

            _context.Baskets.Add(basket);

            return basket;
        }
    }
}