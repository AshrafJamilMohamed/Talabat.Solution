using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helper;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications.ProductSpecs;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : BaseAPIController
    {
        private readonly IUnitOfWork unitOfWork;
       /// private readonly IGenericRepository<Product> productrepository;
       /// private readonly IGenericRepository<ProductBrand> _brandsRepository;
       /// private readonly IGenericRepository<ProductCategory> _categoryRepository;
        private readonly IMapper mapper;

        public ProductsController(
            IUnitOfWork _unitOfWork,
          ///  IGenericRepository<Product> Productrepository,
          ///  IGenericRepository<ProductBrand> brandsRepository,
          ///  IGenericRepository<ProductCategory> categoryRepository,
            IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
         ///   productrepository = Productrepository;
         ///   _brandsRepository = brandsRepository;
         ///   _categoryRepository = categoryRepository;
            mapper = _mapper;

        }


        // Get All Products
        [Authorize]
        [CacheAttribute(600)]
        [HttpGet]
        [ProducesResponseType(typeof(Pagination<ProductToReturnDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProducts([FromQuery] ProductSpecParams productSpecParams)
        {
            var specifications = new ProductWithBrandAndCategorySpec(productSpecParams);
            // Get All Products
            var Products = await unitOfWork.Repository<Product>().GetAllWithSpecAsync(specifications);
            // Mapped These Products
            var MappedProducts = mapper.Map<IEnumerable<ProductToReturnDTO>>(Products);

            // Count Specifications
            var CountSpec = new ProductWithFilterationAndSpec(productSpecParams);
            // Get Count
            var Count = await unitOfWork.Repository<Product>().GetCountAsync(CountSpec);

            var Data = new Pagination<ProductToReturnDTO>(productSpecParams.PageSize, productSpecParams.PageIndex, MappedProducts, Count);

            return Ok(Data);
        }



        // Get Product By Id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDTO?>> GetProduct(int id)
        {
            var specifications = new ProductWithBrandAndCategorySpec(id);

            var Product = await unitOfWork.Repository<Product>().GetWithSpecAsync(specifications);
            if (Product is null) return NotFound(new ApiResponse(404)); // 404
            var MappedProducts = mapper.Map<ProductToReturnDTO>(Product);

            return Ok(MappedProducts); // 200
        }


        #region Brands && Categories

        // Get All Brands
        [HttpGet("brands")]
        [ProducesResponseType(typeof(ProductBrand), StatusCodes.Status200OK)]
        public async Task<IReadOnlyList<ProductBrand>> GetBrands()
            => await unitOfWork.Repository<ProductBrand>().GetAllAsync();


        // Get All Categories
        [HttpGet("categories")]
        [ProducesResponseType(typeof(ProductCategory), StatusCodes.Status200OK)]

        public async Task<IReadOnlyList<ProductCategory>> GetCategories()
           => await unitOfWork.Repository<ProductCategory>().GetAllAsync();


        #endregion


    }
}
