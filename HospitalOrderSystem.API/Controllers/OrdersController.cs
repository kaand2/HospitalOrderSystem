using FluentValidation;
using HospitalOrderSystem.Application.DTOs.Orders;
using HospitalOrderSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalOrderSystem.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IValidator<CreateOrderDto> _createOrderValidator;
        private readonly IValidator<UpdateOrderDto> _updateOrderValidator;
        private readonly IValidator<CancelOrderDto> _cancelOrderValidator;

        public OrdersController(IOrderService orderService,
            IValidator<CreateOrderDto> createOrderValidator,
            IValidator<UpdateOrderDto> updateOrderValidator,
            IValidator<CancelOrderDto> cancelOrderValidator)
        {
            _orderService = orderService;
            _createOrderValidator = createOrderValidator;
            _updateOrderValidator = updateOrderValidator;
            _cancelOrderValidator = cancelOrderValidator;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetAll()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            List<OrderDto> orders = await _orderService.GetAllAsync(userRole);
            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
            OrderDto order = await _orderService.GetByIdAsync(id, userRole);
            return Ok(order);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<ActionResult<OrderDto>> Create(
            [FromBody] CreateOrderDto createOrderDto)
        {
            var validationResult =
                await _createOrderValidator.ValidateAsync(createOrderDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(error => error.ErrorMessage)
                            .ToArray());

                return BadRequest(new
                {
                    errors
                });
            }
            OrderDto createdOrder =
                await _orderService.CreateAsync(createOrderDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdOrder.Id },
                createdOrder);
        }

        [Authorize(Roles = "Doctor,Admin")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<OrderDto>> Update(
            int id,
            [FromBody] UpdateOrderDto updateOrderDto)
        {
            var validationResult =
                await _updateOrderValidator.ValidateAsync(updateOrderDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(error => error.ErrorMessage)
                            .ToArray());

                return BadRequest(new
                {
                    errors
                });
            }
            OrderDto updatedOrder =
                await _orderService.UpdateAsync(
                    id,
                    updateOrderDto);

            return Ok(updatedOrder);
        }

        [Authorize(Roles = "Doctor,Admin")]
        [HttpPut("{id:int}/cancel")]
        public async Task<ActionResult<OrderDto>> Cancel(
            int id,
            [FromBody] CancelOrderDto cancelOrderDto)
        {
            var validationResult =
                await _cancelOrderValidator.ValidateAsync(cancelOrderDto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(error => error.ErrorMessage)
                            .ToArray());

                return BadRequest(new { errors });
            }

            OrderDto cancelledOrder = await _orderService.CancelAsync(id, cancelOrderDto);
            return Ok(cancelledOrder);
        }

        [Authorize(Roles = "Doctor,Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteAsync(id);

            return Ok(new
            {
                message = "Order silindi."
            });
        }
    }
}
