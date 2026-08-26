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

        public async Task<OrderActionDto> CreateAsync(int userId, string userRole, CreateOrderActionDto createDto)
        {
            Order? order = await _orderRepository.GetByIdAsync(createDto.OrderId);
            if (order is null)
            {
                throw new KeyNotFoundException($"Id değeri {createDto.OrderId} olan order bulunamadı.");
            }

            if (!OrderService.IsAuthorizedForOrderType(userRole, order.OrderType))
            {
                throw new UnauthorizedAccessException("Bu order türünde işlem yapma yetkiniz yok.");
            }
            User? user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                throw new KeyNotFoundException($"Id değeri {userId} olan kullanıcı bulunamadı.");
            }

            if (createDto.NewStatus.HasValue && createDto.NewStatus.Value != order.Status)
            {
                OrderService.ValidateStatusTransition(order.Status, createDto.NewStatus.Value);
            }

            OrderAction action = _mapper.Map<OrderAction>(createDto);
            action.UserId = userId;
            action.ActionDate = DateTime.UtcNow;
            action.PreviousStatus = order.Status;

            await _orderActionRepository.AddAsync(action);

            if (createDto.NewStatus.HasValue && createDto.NewStatus.Value != order.Status)
            {
                order.Status = createDto.NewStatus.Value;
                _orderRepository.Update(order);
            }

            // Tek bir işlemde hem action hem de order değişikliklerini kaydet
            await _orderActionRepository.SaveChangesAsync();

            return _mapper.Map<OrderActionDto>(action);
        }
    }
}
