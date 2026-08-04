using HospitalOrderSystem.Domain.Enums;
using System;

namespace HospitalOrderSystem.Application.DTOs.Patients;

public class PatientDto
{
    public int Id { get; set; }
    public string TcNo { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime? UpcomingApp { get; set; }
    public InsuranceType? InsuranceType { get; set; }
    public DateTime CreatedDate { get; set; }
}
