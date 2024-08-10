using AutoMapper;
using Ryze.System.Application.DTO.Tickets;
using Ryze.System.Application.DTO.Users;
using Ryze.System.Web.Models.Accounts;
using Ryze.System.Web.Models.Tickets;

namespace Ryze.System.Web.Mapping
{
    public class ViewModelToDto : Profile
    {
        public ViewModelToDto()
        {
            CreateMap<ApplicationUserDTO, ApplicationUserViewModel>().ReverseMap();
            CreateMap<ApplicationUserDTO, EditProfileViewModel>().ReverseMap();
            CreateMap<TicketDTO, CreateTicketViewModel>().ReverseMap();
            CreateMap<TicketDTO, EditTicketViewModel>().ReverseMap();
            CreateMap<EditTicketViewModel, TicketDTO>().ReverseMap();
            CreateMap<TicketDTO, TicketViewModel>().ReverseMap();
        }
    }
}
