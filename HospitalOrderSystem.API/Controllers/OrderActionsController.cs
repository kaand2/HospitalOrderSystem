using FluentValidation;
using HospitalOrderSystem.Application.DTOs.OrderActions;
using HospitalOrderSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalOrderSystem.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderActionsController : ControllerBase
    {
        private readonly IOrderActionService _orderActionService;
        private readonly IValidator<CreateOrderActionDto> _createOrderActionValidator;

        public OrderActionsController(IOrderActionService orderActionService,
            IValidator<CreateOrderActionDto> createOrderActionValidator)
        {
            _orderActionService = orderActionService;
            _createOrderActionValidator = createOrderActionValidator;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderActionDto>>> GetAll()
        {
            List<OrderActionDto> actions = await _orderActionService.GetAllAsync();
            return Ok(actions);
        }

        [HttpGet("order/{orderId:int}")]
        public async Task<ActionResult<List<OrderActionDto>>> GetByOrderId(int orderId)
        {
            List<OrderActionDto> actions = await _orderActionService.GetByOrderIdAsync(orderId);
            return Ok(actions);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderActionDto>> GetById(int id)
        {
            OrderActionDto action = await _orderActionService.GetByIdAsync(id);
            return Ok(action);
        }

        [HttpPost]
        public async Task<ActionResult<OrderActionDto>> Create(
            [FromBody] CreateOrderActionDto createDto)
        {
            var validationResult =
                await _createOrderActionValidator.ValidateAsync(createDto);
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
            OrderActionDto createdAction =
                await _orderActionService.CreateAsync(createDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdAction.Id },
                createdAction);
        }
    }
}
