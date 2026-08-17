using HospitalOrderSystem.Application.Interfaces.Repositories;
using HospitalOrderSystem.Domain.Entities;
using HospitalOrderSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HospitalOrderSystem.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ProjectDbContext _context;
        public OrderRepository(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Where(order => !order.IsDeleted)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(order => order.Id == id && !order.IsDeleted);
        }

        public async Task<List<Order>> SearchAsync(
            string? patientFirstName,
            string? patientLastName,
            string? patientTcNo,
            string? doctorFirstName,
            string? doctorLastName)
        {
            var query = _context.Orders
                .Include(order => order.Patient)
                .Include(order => order.CreatedByUser)
                .Where(order => !order.IsDeleted);

            if (!string.IsNullOrWhiteSpace(patientFirstName))
                query = query.Where(order =>
                    order.Patient.FirstName.ToLower().Contains(patientFirstName.Trim().ToLower()));

            if (!string.IsNullOrWhiteSpace(patientLastName))
                query = query.Where(order =>
                    order.Patient.LastName.ToLower().Contains(patientLastName.Trim().ToLower()));

            if (!string.IsNullOrWhiteSpace(patientTcNo))
                query = query.Where(order =>
                    order.Patient.TcNo == patientTcNo.Trim());

            if (!string.IsNullOrWhiteSpace(doctorFirstName))
                query = query.Where(order =>
                    order.CreatedByUser.FirstName.ToLower().Contains(doctorFirstName.Trim().ToLower()));

            if (!string.IsNullOrWhiteSpace(doctorLastName))
                query = query.Where(order =>
                    order.CreatedByUser.LastName.ToLower().Contains(doctorLastName.Trim().ToLower()));

            return await query.ToListAsync();
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public void Update(Order order)
        {
            order.UpdatedDate = DateTime.UtcNow;
            _context.Orders.Update(order);
        }

        public void Delete(Order order)
        {
            order.IsDeleted = true;
            order.UpdatedDate = DateTime.UtcNow;

            _context.Orders.Update(order);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
