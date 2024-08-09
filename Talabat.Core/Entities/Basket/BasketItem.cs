namespace Talabat.Core.Entities.Basket
{
    public class BasketItem
    {
         public int Id { get; set; }
        public string ProductName { get; set; } = String.Empty;
        public string PictureUrl { get; set; } = String.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; } = String.Empty;
        public string Brand { get; set; } = String.Empty;

    }
}