using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.OrderAggregation;

namespace Talabat.Core.Service.Contract
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> CreateOrUpdatePaymentIntentAsync(string BasketId);

        Task<Order?> UpdateOrderStatus(string PaymentIntentId, bool IsPaid);

    }
}
