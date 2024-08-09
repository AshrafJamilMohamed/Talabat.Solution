using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregation;

namespace Talabat.Core.Specifications.OrderSpecifications
{
    public class OrderWithPaymentIntentSpec:Specifications<Order>
    {
        public OrderWithPaymentIntentSpec(string PaymentIntentid)
            :base(O=>O.PaymentIntentId == PaymentIntentid)
        {
            
        }
    }
}
