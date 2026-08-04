using AutoMapper;
using HospitalOrderSystem.Application.DTOs.Patients;
using HospitalOrderSystem.Application.Interfaces.Services;
using HospitalOrderSystem.Application.Interfaces.Repositories;
using HospitalOrderSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalOrderSystem.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        public PatientService(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }
        public async Task<List<PatientDto>> GetAllAsync()
        {
            List<Patient> patients = await _patientRepository.GetAllAsync();
            return _mapper.Map<List<PatientDto>>(patients);
        }
        public async Task<PatientDto> GetByIdAsync(int id)
        {
            Patient? patient = await _patientRepository.GetByIdAsync(id);
            if (patient is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan hasta bulunamadı.");
            }
            return _mapper.Map<PatientDto>(patient);
        }
        public async Task<PatientDto> CreateAsync(CreatePatientDto createPatientDto)
        {
            string normalizedTcNo = createPatientDto.TcNo.Trim();
            bool tcNoExists = await _patientRepository.TcNoExistsAsync(normalizedTcNo);
            if (tcNoExists)
            {
                throw new InvalidOperationException("Bu TC Kimlik numarası ile kayıtlı bir hasta zaten bulunmaktadır.");
            }
            Patient patient = _mapper.Map<Patient>(createPatientDto);
            patient.TcNo = normalizedTcNo;
            patient.CreatedDate = DateTime.UtcNow;
            patient.IsDeleted = false;
            patient.UpdatedDate = null;
            patient.DeletedDate = null;
            await _patientRepository.AddAsync(patient);
            await _patientRepository.SaveChangesAsync();
            return _mapper.Map<PatientDto>(patient);
        }
        public async Task<PatientDto> UpdateAsync(int id, UpdatePatientDto updatePatientDto)
        {
            Patient? patient = await _patientRepository.GetByIdAsync(id);
            if (patient is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan hasta bulunamadı.");
            }
            string normalizedTcNo = updatePatientDto.TcNo.Trim();
            bool tcNoExists = await _patientRepository.TcNoExistsAsync(normalizedTcNo, id);
            if (tcNoExists)
            {
                throw new InvalidOperationException("Bu TC Kimlik numarası başka bir hastaya aittir.");
            }
            _mapper.Map(updatePatientDto, patient);
            patient.TcNo = normalizedTcNo;
            _patientRepository.Update(patient);
            await _patientRepository.SaveChangesAsync();
            return _mapper.Map<PatientDto>(patient);
        }
        public async Task DeleteAsync(int id)
        {
            Patient? patient = await _patientRepository.GetByIdAsync(id);
            if (patient is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan hasta bulunamadı.");
            }
            _patientRepository.Delete(patient);
            await _patientRepository.SaveChangesAsync();
        }
    }
}
