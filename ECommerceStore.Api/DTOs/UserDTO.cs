using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceStore.Api.DTOs
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public BasketDTO Basket { get; set; }
    }
}