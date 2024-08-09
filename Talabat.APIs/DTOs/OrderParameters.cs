using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class OrderParameters
    {
        [Required]
        public int DeliveryMethodId { get; set; }
        [Required]
        public string BasketId { get; set; }
        [Required]
        public string BuyerEmail { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string Country { get; set; }

    }
}
