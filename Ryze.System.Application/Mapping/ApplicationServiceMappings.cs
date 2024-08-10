using AutoMapper;
using Ryze.System.Application.DTO.Tickets;
using Ryze.System.Application.DTO.Users;
using Ryze.System.Domain.Entity.Identity;
using Ryze.System.Domain.Entity.Tickets;

namespace Ryze.System.Application.Mapping
{
    public class ApplicationServiceMappings : Profile
    {
        public ApplicationServiceMappings()
        {
            CreateMap<Ticket, TicketDTO>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserDTO>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
              .ForMember(dest => dest.IsClient, opt => opt.MapFrom(src => src.IsClient))
              .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
              .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
              .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar))
              .ForMember(dest => dest.Roles, opt => opt.Ignore())
              .ReverseMap();
        }
    }
}
