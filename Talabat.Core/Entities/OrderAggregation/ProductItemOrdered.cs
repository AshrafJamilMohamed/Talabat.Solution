namespace Talabat.Core.Entities.OrderAggregation
{
    public class ProductItemOrdered 
    {
        // For EFCore
        private ProductItemOrdered()
        {
                
        }
        public ProductItemOrdered(int productId, string productName, string productUrl)
        {
            ProductId = productId;
            ProductName = productName;
            ProductUrl = productUrl;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductUrl { get; set; } = string.Empty;
    }
}
