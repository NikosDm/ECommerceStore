using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ECommerceStore.Api.Entities
{
    public class UserPaymentDetails : PaymentDetails
    {
        public int Id { get; set; }
    }
}