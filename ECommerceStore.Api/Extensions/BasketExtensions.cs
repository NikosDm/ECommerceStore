using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceStore.Api.DTOs;
using ECommerceStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceStore.Api.Extensions
{
    public static class BasketExtensions
    {
        public static BasketDTO MapBasketToDTO(this Basket basket)
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
                    PictureUrl = item.Product.PictureUrl,
                    Description = item.Product.Description,
                    Type = item.Product.Type,
                    Brand = item.Product.Brand,
                    Quantity = item.Quantity
                }).ToList()
            };
        }

        public static IQueryable<Basket> RetrieveBasketWithItems(this IQueryable<Basket> query, string buyerId)
        {
            return query
                .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .Where(b => b.BuyerId == buyerId);
        }
    }
}