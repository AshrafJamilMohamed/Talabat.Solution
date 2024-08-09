using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecs
{
    public class ProductWithFilterationAndSpec : Specifications<Product>
    {
        public ProductWithFilterationAndSpec(ProductSpecParams productSpecParams)
             : base(P =>
                     (string.IsNullOrEmpty(productSpecParams.Search) || P.Name.ToLower().Contains(productSpecParams.Search))
                                              &&
                    (!productSpecParams.CategoryId.HasValue || productSpecParams.CategoryId.Value == P.CategoryId)
                                              &&
                        (!productSpecParams.BrandId.HasValue || productSpecParams.BrandId.Value == P.BrandId)
                    )
        {

        }
    }
}
