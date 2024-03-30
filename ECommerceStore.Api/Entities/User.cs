using Microsoft.AspNetCore.Identity;

namespace ECommerceStore.Api.Entities
{
    public class User : IdentityUser<int>
    {
        public UserAddress Address { get; set; }
        public UserPaymentDetails PaymentDetails { get; set; }
    }
}