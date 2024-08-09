namespace Talabat.Core.Entities.Identity
{
    public class Address : BaseEntity
    {
        public  string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}