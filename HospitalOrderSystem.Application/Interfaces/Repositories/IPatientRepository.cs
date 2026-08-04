using HospitalOrderSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalOrderSystem.Application.Interfaces.Repositories
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(int id);
        Task<Patient?> GetByTcNoAsync(string tcNo);
        Task<bool> TcNoExistsAsync(string tcNo, int? excludedPatientId = null);
        Task AddAsync(Patient patient);
        void Update(Patient patient);
        void Delete(Patient patient);
        Task<int> SaveChangesAsync();
    }
}
