using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceStore.Api.Entities;

namespace ECommerceStore.Api.DTOs
{
    public class BasketDTO
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public List<BasketItemDTO> Items { get; set; }
    }
}