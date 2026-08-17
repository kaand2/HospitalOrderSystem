using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalOrderSystem.Application.DTOs.Patients;

namespace HospitalOrderSystem.Application.Interfaces.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<PatientDto> GetByIdAsync(int id);
        Task<List<PatientDto>> SearchAsync(string? firstName, string? lastName, string? tcNo);
        Task<PatientDto> CreateAsync(CreatePatientDto createPatientDto);
        Task<PatientDto> UpdateAsync(int id, UpdatePatientDto updatePatientDto);
        Task DeleteAsync(int id);
    }
}
