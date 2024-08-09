using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.OrderAggregation;
using Talabat.Core.Service.Contract;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class OrdersController : BaseAPIController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService _orderService, IMapper _mapper)
        {
            orderService = _orderService;
            mapper = _mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrderAsync(OrderDTO orderDto)
        {

            var MappedAddress = new Address
            {
                FirstName = orderDto.ShippingAddress.FirstName,
                LastName = orderDto.ShippingAddress.LastName,
                City = orderDto.ShippingAddress.City,
                Street = orderDto.ShippingAddress.Street,
                Country = orderDto.ShippingAddress.Country

            };
            var Order = await orderService.CreateOrderAsync(MappedAddress, orderDto.BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId,orderDto.PaymentIntentId);

            if (Order is not null) return Ok(mapper.Map<Order, OrderToReturnDto>(Order));
            return Ok(BadRequest(new ApiResponse(400)));


        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders(string buyerEmail)
          => Ok
            (mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDto>>
              (await orderService.GetOrdersForUserAsync(buyerEmail))
              );


        [ProducesResponseType(typeof(OrderToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetUserOrder(int id, string buyerEmail)
        {
            var Order = await orderService.GetOrderByIdForUserAsync(buyerEmail, id);
            if (Order is null) return Ok(NotFound(new ApiResponse(404)));
            return Ok(mapper.Map<OrderToReturnDto>(Order));
        }


        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethodsAsync()
             => Ok(await orderService.GetDeliveryMethodsAsync());

    }
}
