using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIs.Controllers
{

    public class BasketController : BaseAPIController
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;
         
        public BasketController(IBasketRepository _basketRepository, IMapper _mapper)
        {

            basketRepository = _basketRepository;
            mapper = _mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketAsync(string id)
        {
            var Basket = await basketRepository.GetBasketAsync(id);
            return Ok(Basket is null ? new CustomerBasket(id) : Basket);
        }

        [HttpPost] 
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasketAsync(CustomerBasketDTO customerBasket)
        {
            var mappedbasket = mapper.Map<CustomerBasket>(customerBasket);
            var Basket = await basketRepository.UpdateBasketAsync(mappedbasket);
            return Ok(Basket is null ? BadRequest(new ApiResponse(400)) : Basket);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasketAsync(string Id)
           => await basketRepository.DeleteBasketAsync(Id);




    }
}
