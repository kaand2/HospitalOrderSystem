using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HospitalOrderSystem.Application.Interfaces.Services;
using HospitalOrderSystem.Application.Services;
using HospitalOrderSystem.Application.Validators.Patients;
using Microsoft.Extensions.DependencyInjection;
using HospitalOrderSystem.Application.Mappings;

namespace HospitalOrderSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IPatientService, PatientService>();
            services.AddValidatorsFromAssemblyContaining<CreatePatientDtoValidator>();
            services.AddAutoMapper(configuration => { }, typeof(PatientMappingProfile));
            return services;
        }
    }
}