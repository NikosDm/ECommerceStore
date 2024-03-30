using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceStore.Api.Entities;
using ECommerceStore.Api.Entities.OrderAggregate;

namespace ECommerceStore.Api.DTOs
{
    public class CreateOrderDTO
    {
        public bool SaveAddress { get; set; }
        public bool SaveCard { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public OrderPaymentDetails PaymentDetails { get; set; }
    }
}