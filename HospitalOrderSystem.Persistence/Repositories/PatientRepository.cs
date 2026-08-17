using HospitalOrderSystem.Application.Interfaces.Repositories;
using HospitalOrderSystem.Domain.Entities;
using HospitalOrderSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalOrderSystem.Persistence.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly ProjectDbContext _context;
    public PatientRepository(ProjectDbContext context)
    {
        _context = context;
    }
    public async Task<List<Patient>> GetAllAsync()
    {
        return await _context.Patients
            .Where(patient => !patient.IsDeleted)
            .ToListAsync();
    }
    public async Task<Patient?> GetByIdAsync(int id)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(patient =>
                patient.Id == id &&
                !patient.IsDeleted);
    }
    public async Task<Patient?> GetByTcNoAsync(string tcNo)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(patient =>
                patient.TcNo == tcNo &&
                !patient.IsDeleted);
    }
    public async Task<List<Patient>> SearchAsync(string? firstName, string? lastName, string? tcNo)
    {
        var query = _context.Patients.Where(patient => !patient.IsDeleted);

        if (!string.IsNullOrWhiteSpace(firstName))
            query = query.Where(patient =>
                patient.FirstName.ToLower().Contains(firstName.Trim().ToLower()));

        if (!string.IsNullOrWhiteSpace(lastName))
            query = query.Where(patient =>
                patient.LastName.ToLower().Contains(lastName.Trim().ToLower()));

        if (!string.IsNullOrWhiteSpace(tcNo))
            query = query.Where(patient =>
                patient.TcNo == tcNo.Trim());

        return await query.ToListAsync();
    }
    public async Task<bool> TcNoExistsAsync(string tcNo, int? excludedPatientId = null)
    {
        return await _context.Patients.AnyAsync(patient =>
            patient.TcNo == tcNo && (!excludedPatientId.HasValue || patient.Id != excludedPatientId.Value));
    }
    public async Task AddAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
    }
    public void Update(Patient patient)
    {
        patient.UpdatedDate = DateTime.UtcNow;
        _context.Patients.Update(patient);
    }
    public void Delete(Patient patient)
    {
        patient.IsDeleted = true;
        patient.DeletedDate = DateTime.UtcNow;
        patient.UpdatedDate = DateTime.UtcNow;

        _context.Patients.Update(patient);
    }
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
