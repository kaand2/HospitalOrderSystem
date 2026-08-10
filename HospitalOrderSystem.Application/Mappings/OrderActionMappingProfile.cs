using AutoMapper;
using HospitalOrderSystem.Application.DTOs.OrderActions;
using HospitalOrderSystem.Domain.Entities;

namespace HospitalOrderSystem.Application.Mappings
{
    public class OrderActionMappingProfile : Profile
    {
        public OrderActionMappingProfile()
        {
            CreateMap<OrderAction, OrderActionDto>();

            CreateMap<CreateOrderActionDto, OrderAction>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PreviousStatus, opt => opt.Ignore())
                .ForMember(dest => dest.ActionDate, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
