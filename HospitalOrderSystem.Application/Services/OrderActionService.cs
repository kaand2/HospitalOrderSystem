using AutoMapper;
using HospitalOrderSystem.Application.DTOs.OrderActions;
using HospitalOrderSystem.Application.Interfaces.Repositories;
using HospitalOrderSystem.Application.Interfaces.Services;
using HospitalOrderSystem.Domain.Entities;

namespace HospitalOrderSystem.Application.Services
{
    public class OrderActionService : IOrderActionService
    {
        private readonly IOrderActionRepository _orderActionRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public OrderActionService(
            IOrderActionRepository orderActionRepository,
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _orderActionRepository = orderActionRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderActionDto>> GetAllAsync()
        {
            List<OrderAction> actions = await _orderActionRepository.GetAllAsync();
            return _mapper.Map<List<OrderActionDto>>(actions);
        }

        public async Task<List<OrderActionDto>> GetByOrderIdAsync(int orderId)
        {
            List<OrderAction> actions = await _orderActionRepository.GetByOrderIdAsync(orderId);
            return _mapper.Map<List<OrderActionDto>>(actions);
        }

        public async Task<OrderActionDto> GetByIdAsync(int id)
        {
            OrderAction? action = await _orderActionRepository.GetByIdAsync(id);
            if (action is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan işlem (aksiyon) bulunamadı.");
            }
            return _mapper.Map<OrderActionDto>(action);
        }

        public async Task<OrderActionDto> CreateAsync(CreateOrderActionDto createDto)
        {
            Order? order = await _orderRepository.GetByIdAsync(createDto.OrderId);
            if (order is null)
            {
                throw new KeyNotFoundException($"Id değeri {createDto.OrderId} olan order bulunamadı.");
            }

            User? user = await _userRepository.GetByIdAsync(createDto.UserId);
            if (user is null)
            {
                throw new KeyNotFoundException($"Id değeri {createDto.UserId} olan kullanıcı bulunamadı.");
            }

            OrderAction action = _mapper.Map<OrderAction>(createDto);
            action.ActionDate = DateTime.UtcNow;
            action.PreviousStatus = order.Status;

            await _orderActionRepository.AddAsync(action);
            await _orderActionRepository.SaveChangesAsync();

            if (createDto.NewStatus.HasValue && createDto.NewStatus.Value != order.Status)
            {
                order.Status = createDto.NewStatus.Value;
                _orderRepository.Update(order);
                await _orderRepository.SaveChangesAsync();
            }

            return _mapper.Map<OrderActionDto>(action);
        }
    }
}
