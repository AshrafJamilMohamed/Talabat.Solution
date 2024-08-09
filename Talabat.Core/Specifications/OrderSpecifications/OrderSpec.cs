using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregation;

namespace Talabat.Core.Specifications.OrderSpecifications
{
    public class OrderSpec : Specifications<Order>
    {
        public OrderSpec(string BuyerEmail)
            : base(O => O.BuyerEmail == BuyerEmail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            OrderByDesc = (O => O.OrderDate);
        }
          
        public OrderSpec(int OrderId, string buyerEmail)
            :base(O=>O.Id == OrderId && O.BuyerEmail == buyerEmail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
