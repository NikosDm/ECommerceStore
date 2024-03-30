using ECommerceStore.Api.Entities.OrderAggregate;

namespace ECommerceStore.Api.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public double Subtotal { get; set; }
        public double DeliveryFee { get; set; }
        public string OrderStatus { get; set; }
        public double Total { get; set; }
    }
}