using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregation;
using Talabat.Repository.Identity;

namespace Talabat.Repository.Data
{
    public static class DataSeeding
    {
        public static async Task SeedData(StoreContext DbContext)
        {
            #region ProductBrand 

            if (DbContext.ProductBrands.Count() == 0)
            {

                var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);

                if (Brands is not null && Brands.Count() > 0)
                {
                    foreach (var Brand in Brands)
                    {
                        await DbContext.Set<ProductBrand>().AddAsync(Brand);
                    }
                    await DbContext.SaveChangesAsync();

                }
            }

            #endregion

            #region ProductCategory 

            if (DbContext.ProductCategories.Count() == 0)
            {

                var CategoriesData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/categories.json");
                var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoriesData);

                if (Categories is not null && Categories.Count() > 0)
                {
                    foreach (var category in Categories)
                    {
                        await DbContext.Set<ProductCategory>().AddAsync(category);
                    }
                    await DbContext.SaveChangesAsync();

                }
            }

            #endregion

            #region Product 

            if (DbContext.Products.Count() == 0)
            {

                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                if (/*Products is not null && Products.Count() > 0*/ Products?.Count() > 0)
                {
                    foreach (var product in Products)
                    {
                        await DbContext.Set<Product>().AddAsync(product);
                    }
                    await DbContext.SaveChangesAsync();

                }
            }

            #endregion

            #region DeliveryMethod

            if (DbContext.DeliveryMethods.Count() == 0)
            {
                var DeliveryMethodData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/delivery.json");

                var DeliveryMethod = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);
                if (DeliveryMethod?.Count() > 0)
                {
                    foreach (var item in DeliveryMethod)
                        await DbContext.DeliveryMethods.AddAsync(item);
                    await DbContext.SaveChangesAsync();
                }
            }

            #endregion

        }
    }
}
