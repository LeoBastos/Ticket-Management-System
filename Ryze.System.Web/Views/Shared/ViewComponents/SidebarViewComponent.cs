using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ryze.System.Application.Services.Tickets;
using Ryze.System.Domain.Entity.Identity;

namespace Ryze.System.Web.Views.Shared.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly ITicketService _ticketService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SidebarViewComponent(ITicketService ticketService, UserManager<ApplicationUser> userManager)
        {
            _ticketService = ticketService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            //if (user == null) return View(new Dictionary<StatusEnum, int>());

            //var ticketCounts = await _ticketService.GetTicketCountsByUserId(user.Id);
            return View(user);
        }
    }
}
