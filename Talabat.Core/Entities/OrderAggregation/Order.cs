namespace Talabat.Core.Entities.OrderAggregation
{
    public class Order : BaseEntity
    {
        // For EFCore
        private Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, ICollection<OrderItem> items, DeliveryMethod? deliveryMethod, decimal subTotal,string PaymentIntentid)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            Items = items;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
            PaymentIntentId = PaymentIntentid;
        }

        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public Address ShippingAddress { get; set; } = null!;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();

        //public int DeliveryMethodId { get; set; } // FK
        public DeliveryMethod? DeliveryMethod { get; set; } = null!;

        public decimal SubTotal { get; set; }

        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;

        public string PaymentIntentId { get; set; }  

    }
}
