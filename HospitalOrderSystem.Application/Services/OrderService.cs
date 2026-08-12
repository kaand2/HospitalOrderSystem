using AutoMapper;
using HospitalOrderSystem.Application.DTOs.Orders;
using HospitalOrderSystem.Application.Interfaces.Repositories;
using HospitalOrderSystem.Application.Interfaces.Services;
using HospitalOrderSystem.Domain.Entities;
using HospitalOrderSystem.Domain.Enums;

namespace HospitalOrderSystem.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            IPatientRepository patientRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _patientRepository = patientRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> GetAllAsync(string userRole)
        {
            List<Order> orders = await _orderRepository.GetAllAsync();
            var filteredOrders = orders.Where(o => IsAuthorizedForOrderType(userRole, o.OrderType)).ToList();
            return _mapper.Map<List<OrderDto>>(filteredOrders);
        }

        public async Task<OrderDto> GetByIdAsync(int id, string userRole)
        {
            Order? order = await _orderRepository.GetByIdAsync(id);
            if (order is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan order bulunamadı.");
            }

            if (!IsAuthorizedForOrderType(userRole, order.OrderType))
            {
                throw new UnauthorizedAccessException("Bu departmanın siparişlerini görüntüleme yetkiniz yok.");
            }

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDto createOrderDto)
        {
            Patient? patient = await _patientRepository.GetByIdAsync(createOrderDto.PatientId);
            if (patient is null)
            {
                throw new KeyNotFoundException($"Id değeri {createOrderDto.PatientId} olan hasta bulunamadı.");
            }

            User? user = await _userRepository.GetByIdAsync(createOrderDto.CreatedByUserId);
            if (user is null)
            {
                throw new KeyNotFoundException($"Id değeri {createOrderDto.CreatedByUserId} olan kullanıcı bulunamadı.");
            }

            Order order = _mapper.Map<Order>(createOrderDto);
            order.CreatedDate = DateTime.UtcNow;
            order.IsDeleted = false;
            order.UpdatedDate = null;
            order.CompletedDate = null;
            order.CancelledDate = null;
            order.CancellationReason = null;
            order.IsCancelled = false;
            order.Status = OrderStatus.Draft;

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateAsync(int id, UpdateOrderDto updateOrderDto)
        {
            Order? order = await _orderRepository.GetByIdAsync(id);
            if (order is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan order bulunamadı.");
            }

            OrderStatus previousStatus = order.Status;
            OrderStatus newStatus = updateOrderDto.Status;

            if (previousStatus != newStatus)
            {
                ValidateStatusTransition(previousStatus, newStatus);
            }

            _mapper.Map(updateOrderDto, order);

            if (previousStatus != newStatus)
            {
                if (newStatus == OrderStatus.Completed)
                {
                    order.CompletedDate = DateTime.UtcNow;
                }
                else if (newStatus == OrderStatus.Cancelled)
                {
                    order.CancelledDate = DateTime.UtcNow;
                }
            }

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CancelAsync(int id, CancelOrderDto cancelOrderDto)
        {
            Order? order = await _orderRepository.GetByIdAsync(id);
            if (order is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan order bulunamadı.");
            }

            OrderStatus previousStatus = order.Status;
            ValidateStatusTransition(previousStatus, OrderStatus.Cancelled);

            order.Status = OrderStatus.Cancelled;
            order.CancellationReason = cancelOrderDto.CancellationReason;
            order.CancelledDate = DateTime.UtcNow;
            order.IsCancelled = true;

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();

            return _mapper.Map<OrderDto>(order);
        }

        public async Task DeleteAsync(int id)
        {
            Order? order = await _orderRepository.GetByIdAsync(id);
            if (order is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan order bulunamadı.");
            }

            _orderRepository.Delete(order);
            await _orderRepository.SaveChangesAsync();
        }

        private static void ValidateStatusTransition(OrderStatus from, OrderStatus to)
        {
            bool isValid = from switch
            {
                OrderStatus.Draft => to == OrderStatus.Active || to == OrderStatus.Cancelled,
                OrderStatus.Active => to == OrderStatus.InProgress || to == OrderStatus.Paused || to == OrderStatus.Cancelled,
                OrderStatus.InProgress => to == OrderStatus.Completed || to == OrderStatus.Cancelled,
                OrderStatus.Paused => to == OrderStatus.Restarted || to == OrderStatus.Cancelled,
                OrderStatus.Restarted => to == OrderStatus.Active,
                OrderStatus.Completed => false,
                OrderStatus.Cancelled => false,
                _ => false
            };

            if (!isValid)
            {
                throw new InvalidOperationException(
                    $"order durumu '{from}' iken '{to}' durumuna geçiş yapılamaz.");
            }
        }

        private static bool IsAuthorizedForOrderType(string userRole, OrderType orderType)
        {
            if (userRole == "Admin" || userRole == "Doctor")
            {
                return true;
            }

            if (userRole == "Nurse")
            {
                return orderType == OrderType.Nursing || orderType == OrderType.Medication || orderType == OrderType.Diet;
            }

            if (userRole == "Laboratory")
            {
                return orderType == OrderType.Laboratory;
            }

            if (userRole == "Radiology")
            {
                return orderType == OrderType.Radiology;
            }

            return false;
        }
    }
}
