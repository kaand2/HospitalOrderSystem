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
using HospitalOrderSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HospitalOrderSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderActionService, OrderActionService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddValidatorsFromAssemblyContaining<CreatePatientDtoValidator>();
            services.AddAutoMapper(configuration => { }, typeof(PatientMappingProfile), typeof(UserMappingProfile), typeof(OrderMappingProfile), typeof(OrderActionMappingProfile), typeof(AppointmentMappingProfile));
            return services;
        }
    }
}