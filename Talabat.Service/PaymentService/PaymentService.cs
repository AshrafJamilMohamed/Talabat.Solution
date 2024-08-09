using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.OrderAggregation;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Service.Contract;
using Talabat.Core.Specifications.OrderSpecifications;

namespace Talabat.Service.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IBasketRepository basketRepo;
        private readonly IUnitOfWork unitOfWork;

        public PaymentService(
            IConfiguration _configuration,
            IBasketRepository _basketRepo,
            IUnitOfWork _unitOfWork
            )
        {
            configuration = _configuration;
            basketRepo = _basketRepo;
            unitOfWork = _unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntentAsync(string BasketId)
        {
            // Congifure SecretKey
            StripeConfiguration.ApiKey = configuration["Strip:Secretkey"];
            // Get Basket
            var Basket = await basketRepo.GetBasketAsync(BasketId);
            if (Basket is null) return null;

            // Check on DeliveryMethodId if is set
            var shippingPrice = 0m;
            if (Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetAsync(Basket.DeliveryMethodId.Value);
                shippingPrice = DeliveryMethod.Cost;
            }

            // Check on the price of each item in basket 

            if (Basket.Items.Count > 0)
            {
                var ProductRepo = unitOfWork.Repository<Core.Entities.Product>();
                foreach (var item in Basket.Items)
                {
                    // Get The Product From DataBase
                    var Product = await ProductRepo.GetAsync(item.Id);
                    // Check on the price
                    if (Product.Price != item.Price)
                        item.Price = Product.Price; // Set the Right Price 
                }
            }

            PaymentIntent paymentIntent;
            PaymentIntentService paymentIntentService = new PaymentIntentService();

            if (string.IsNullOrEmpty(Basket.PaymentIntentId)) // Create new PaymentIntent
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)Basket.Items.Sum(I => I.Price * 100 * I.Quantity) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                // Integration With Strip
                paymentIntent = await paymentIntentService.CreateAsync(Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update Excisting PaymentIntent
            {
                var Oprions = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)Basket.Items.Sum(I => I.Price * 100 * I.Quantity) + (long)shippingPrice * 100,

                };

                // Update The Amount 
                await paymentIntentService.UpdateAsync(Basket.PaymentIntentId, Oprions);
            }

            await basketRepo.UpdateBasketAsync(Basket);
            return Basket;

        }

        public async Task<Order?> UpdateOrderStatus(string PaymentIntentId, bool IsPaid)
        {
            var Spec = new OrderWithPaymentIntentSpec(PaymentIntentId);
            var Order = await unitOfWork.Repository<Order>().GetWithSpecAsync(Spec);
            if (Order is null) return null;

            if (IsPaid)
                Order.Status = OrderStatus.PaymentReceived;
            else
                Order.Status = OrderStatus.PaymentFaild;

            unitOfWork.Repository<Order>().Update(Order);
            await unitOfWork.CompleteAsync();

            return Order;   
        }
    }
}
