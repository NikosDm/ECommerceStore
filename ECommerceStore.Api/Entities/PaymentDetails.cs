using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ECommerceStore.Api.Entities
{
    public class PaymentDetails
    {
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string ExpDate { get; set; }
        public string Cvv { get; set; }
    }
}