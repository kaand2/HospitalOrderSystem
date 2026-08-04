using AutoMapper;
using HospitalOrderSystem.Application.DTOs.Patients;
using HospitalOrderSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalOrderSystem.Application.Mappings
{
    public class PatientMappingProfile: Profile
    {
        public PatientMappingProfile()
        {
            CreateMap<Patient, PatientDto>();
            CreateMap<CreatePatientDto, Patient>()
                .ForMember(destination => destination.Id, options => options.Ignore())
                .ForMember(destination => destination.CreatedDate, options => options.Ignore())
                .ForMember(destination => destination.UpdatedDate, options => options.Ignore())
                .ForMember(destination => destination.IsDeleted, options => options.Ignore())
                .ForMember(destination => destination.DeletedDate, options => options.Ignore())
                .ForMember(destination => destination.Orders, options => options.Ignore());
            CreateMap<UpdatePatientDto, Patient>()
                .ForMember(destination => destination.Id, options => options.Ignore())
                .ForMember(destination => destination.CreatedDate, options => options.Ignore())
                .ForMember(destination => destination.UpdatedDate, options => options.Ignore())
                .ForMember(destination => destination.IsDeleted, options => options.Ignore())
                .ForMember(destination => destination.DeletedDate, options => options.Ignore())
                .ForMember(destination => destination.Orders, options => options.Ignore());
        }
    }
}
