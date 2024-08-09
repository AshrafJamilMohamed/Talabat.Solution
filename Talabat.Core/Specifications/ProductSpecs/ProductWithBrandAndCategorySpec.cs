using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecs
{
    public class ProductWithBrandAndCategorySpec : Specifications<Product>
    {


        // This Ctor For Get All Products
        public ProductWithBrandAndCategorySpec(ProductSpecParams productSpecParams)
            : base(P =>
                        (string.IsNullOrEmpty(productSpecParams.Search) ||  P.Name.ToLower().Contains(productSpecParams.Search) )
                                              &&
                        (!productSpecParams.CategoryId.HasValue || productSpecParams.CategoryId.Value == P.CategoryId)
                                              &&
                        (!productSpecParams.BrandId.HasValue || productSpecParams.BrandId.Value == P.BrandId)
                  )
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);

            if (!string.IsNullOrEmpty(productSpecParams.Sort))
            {
                switch (productSpecParams.Sort)
                {
                    case "price":
                        OrderBy = P => P.Price;
                        break;

                    case "pricedesc":
                        OrderByDesc = P => P.Price;
                        break;

                    default:
                        OrderBy = P => P.Name;
                        break;
                }
            }
            else
                OrderBy = P => P.Name;

            if (productSpecParams.PageIndex > 0 && productSpecParams.PageSize > 0)
            {
                Skip = (productSpecParams.PageIndex - 1) * (productSpecParams.PageSize);

                Take = productSpecParams.PageSize;
                IsPaginationEnabled = true;
            }


        }

        // This Ctor For Get Specific Product
        public ProductWithBrandAndCategorySpec(int id) : base(P => P.Id == id)
        {

            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);

        }




    }
}
