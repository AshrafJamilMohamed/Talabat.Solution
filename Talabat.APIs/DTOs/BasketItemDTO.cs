using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class BasketItemDTO
    {

        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; } = String.Empty;
        [Required]
        public string PictureUrl { get; set; } = String.Empty;
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must greater than zero.")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity Must be 1 item at least")]
        public int Quantity { get; set; }
        [Required]
        public string Category { get; set; } = String.Empty;

        [Required]
        public string Brand { get; set; } = String.Empty;
    }
}
