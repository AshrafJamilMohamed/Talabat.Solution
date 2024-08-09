using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregation;

namespace Talabat.Core.Service.Contract
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(Address shippingAddress, string buyerEmail, string basketId, int deliveryMethodId,string PaymentIntentid);

        Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int OrderId);
        Task<IEnumerable<Order>> GetOrdersForUserAsync(string buyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();


    }
}
