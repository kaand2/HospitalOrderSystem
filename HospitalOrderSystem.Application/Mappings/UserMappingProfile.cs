using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HospitalOrderSystem.Application.DTOs.Users;
using HospitalOrderSystem.Domain.Entities;

namespace HospitalOrderSystem.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>()
                .ForMember(destination => destination.Id, options => options.Ignore())
                .ForMember(destination => destination.PasswordHash, options => options.Ignore())
                .ForMember(destination => destination.IsDeleted, options => options.Ignore())
                .ForMember(destination => destination.DeletedDate, options => options.Ignore())
                .ForMember(destination => destination.CreatedDate, options => options.Ignore())
                .ForMember(destination => destination.UpdatedDate, options => options.Ignore())
                .ForMember(destination => destination.CreatedOrders, options => options.Ignore())
                .ForMember(destination => destination.Actions, options => options.Ignore());
            CreateMap<UpdateUserDto, User>()
                .ForMember(destination => destination.Id, options => options.Ignore())
                .ForMember(destination => destination.PasswordHash, options => options.Ignore())
                .ForMember(destination => destination.CreatedDate, options => options.Ignore())
                .ForMember(destination => destination.UpdatedDate, options => options.Ignore())
                .ForMember(destination => destination.DeletedDate, options => options.Ignore())
                .ForMember(destination => destination.CreatedOrders, options => options.Ignore())
                .ForMember(destination => destination.Actions, options => options.Ignore());
        }
    }
}
