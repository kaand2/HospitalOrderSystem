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

        public async Task<List<OrderDto>> GetAllAsync()
        {
            List<Order> orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetByIdAsync(int id)
        {
            Order? order = await _orderRepository.GetByIdAsync(id);
            if (order is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan sipariş bulunamadı.");
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
                throw new KeyNotFoundException($"Id değeri {id} olan sipariş bulunamadı.");
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

        public async Task DeleteAsync(int id)
        {
            Order? order = await _orderRepository.GetByIdAsync(id);
            if (order is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan sipariş bulunamadı.");
            }

            _orderRepository.Delete(order);
            await _orderRepository.SaveChangesAsync();
        }

        private static void ValidateStatusTransition(OrderStatus from, OrderStatus to)
        {
            bool isValid = from switch
            {
                OrderStatus.Draft => to == OrderStatus.Pending || to == OrderStatus.Cancelled,
                OrderStatus.Pending => to == OrderStatus.Approved || to == OrderStatus.Cancelled,
                OrderStatus.Approved => to == OrderStatus.Completed || to == OrderStatus.Cancelled,
                OrderStatus.Completed => false,
                OrderStatus.Cancelled => false,
                _ => false
            };

            if (!isValid)
            {
                throw new InvalidOperationException(
                    $"Sipariş durumu '{from}' iken '{to}' durumuna geçiş yapılamaz.");
            }
        }
    }
}
