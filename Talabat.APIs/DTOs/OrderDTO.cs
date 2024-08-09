using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.OrderAggregation;

namespace Talabat.APIs.DTOs
{
    public class OrderDTO
    {
        [Required]
        public int DeliveryMethodId { get; set; }
        [Required]
        public string BasketId { get; set; }
        [Required]
        public string BuyerEmail { get; set; }
        [Required]
        public string PaymentIntentId { get; set; }
        public ShippingAddressDTO  ShippingAddress { get; set; }
    }
}
