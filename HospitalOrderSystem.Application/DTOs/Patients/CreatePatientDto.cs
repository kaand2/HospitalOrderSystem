using HospitalOrderSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HospitalOrderSystem.Application.DTOs.Patients;

public class CreatePatientDto
{
    public string TcNo { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime? UpcomingAppointment { get; set; }
    public InsuranceType? InsuranceType { get; set; }
}


