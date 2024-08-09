using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregation;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Service.Contract;
using Talabat.Core.Specifications.OrderSpecifications;

namespace Talabat.Service.OrderServiceModule
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPaymentService paymentService;

        //private readonly IGenericRepository<Product> productRepository;
        //private readonly IGenericRepository<DeliveryMethod> deliveryMethodRepository;
        //private readonly IGenericRepository<Order> orderRepository;

        public OrderService(
            IBasketRepository _basketRepository,
            IUnitOfWork _unitOfWork,
            IPaymentService _paymentService
            ///IGenericRepository<Product> _ProductRepository,
            ///IGenericRepository<DeliveryMethod> _DeliveryMethodRepository,
            ///IGenericRepository<Order> _OrderRepository
            )
        {
            basketRepository = _basketRepository;
            unitOfWork = _unitOfWork;
            paymentService = _paymentService;
            ///productRepository = _ProductRepository;
            ///deliveryMethodRepository = _DeliveryMethodRepository;
            ///orderRepository = _OrderRepository;
        }
        public async Task<Order?> CreateOrderAsync(Address shippingAddress, string buyerEmail, string basketId, int deliveryMethodId, string PaymentIntentid)
        {
            // Get Basket

            var Basket = await basketRepository.GetBasketAsync(basketId);

            // Get Items at the basket

            var OrderItems = new List<OrderItem>();

            if (Basket?.Items?.Count > 0)
            {
                foreach (var item in Basket.Items)
                {

                    var Product = await unitOfWork.Repository<Product>().GetAsync(item.Id);

                    var ProductItemOrdered = new ProductItemOrdered(Product.Id, Product.Name, Product.PictureUrl);

                    var orderItem = new OrderItem(ProductItemOrdered, Product.Price, item.Quantity);
                    OrderItems.Add(orderItem);


                }

                // Calculate Subtotal
                var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);

                // Get DeliveryMethod 

                var DeliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

                // Check if there is another Existing PaymentIntent
                var OrderRepo = unitOfWork.Repository<Order>();
                var Spec = new OrderWithPaymentIntentSpec(PaymentIntentid);
                var ExistingOrder = await OrderRepo.GetWithSpecAsync(Spec);

                if (ExistingOrder is not null)
                {
                    OrderRepo.Delete(ExistingOrder);

                    // Update PaymentIntent with new Quantity
                    await paymentService.CreateOrUpdatePaymentIntentAsync(basketId);
                }
                // Create Order
                var Order = new Order
                    (
                      buyerEmail, shippingAddress, OrderItems, DeliveryMethod, SubTotal, PaymentIntentid
                    );


                unitOfWork.Repository<Order>().Add(Order);
                // Save At Database
                var Result = await unitOfWork.CompleteAsync();
                if (Result <= 0) return null;

                return Order;
            }

            return null;
        }


        public Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int OrderId)
        {
            // Another Way
            // return  unitOfWork.Repository<Order>().GetWithSpecAsync(new OrderSpec(OrderId, buyerEmail));

            var OrderRepo = unitOfWork.Repository<Order>();
            var Spec = new OrderSpec(OrderId, buyerEmail);
            var Order = OrderRepo.GetWithSpecAsync(Spec);
            return Order;
        }

        public Task<IEnumerable<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var OrderRepo = unitOfWork.Repository<Order>();

            var Spec = new OrderSpec(buyerEmail);
            var Orders = OrderRepo.GetAllWithSpecAsync(Spec);
            return Orders;
        }
        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
          => await unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

    }
}
