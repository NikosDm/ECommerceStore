namespace ECommerceStore.Api.Entities
{
    public class Basket
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        
        public void AddItem(Product product, int quantity)
        {
            if (Items.All(item => item.Product.Id != product.Id)) 
            {
                Items.Add(new BasketItem { Product = product, Quantity = quantity });
            }

            var existingItem = Items.FirstOrDefault(item => item.ProductId == product.Id);

            if (existingItem is not null) existingItem.Quantity += quantity;
        }

        public void RemoveItem(int productId, int quantity)
        {
            var item = Items.FirstOrDefault(item => item.ProductId == productId);

            if (item is null) return;

            item.Quantity -= quantity;

            if (item.Quantity == 0) Items.Remove(item);
        }
    }
}