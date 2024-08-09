using Talabat.Core.Entities.OrderAggregation;

namespace Talabat.APIs.DTOs
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }

        public string BuyerEmail { get; set; }  
        public DateTimeOffset OrderDate { get; set; } 
        public Address ShippingAddress { get; set; }  
        public string Status { get; set; }  

        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();

        
        public string? DeliveryMethod { get; set; }  
        public string? DeliveryMethodCost { get; set; }  

        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

        public string PaymentIntentId { get; set; } = string.Empty;
    }
}
