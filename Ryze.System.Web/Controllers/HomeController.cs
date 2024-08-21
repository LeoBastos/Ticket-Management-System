using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ryze.System.Application.DTO.Tickets;
using Ryze.System.Application.Services.Tickets;
using Ryze.System.Domain.Entity.Identity;
using Ryze.System.Web.Models;
using System.Diagnostics;
using System.Numerics;

namespace Ryze.System.Web.Controllers
{

    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;       
        private readonly ITicketService _ticketService;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ITicketService ticketService)
            : base(userManager)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public async Task<IActionResult> Index()
        {
            TicketCountsResult viewModel;

            if (TempData["UpdatedTicketCounts"] != null)
            {
                viewModel = TempData["UpdatedTicketCounts"] as TicketCountsResult;
            }
            else
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                if (User.IsInRole("Cliente"))
                {
                    viewModel = await _ticketService.GetTicketDashboardCountsByClientId(user.Id);
                }
                else if (User.IsInRole("Funcionario"))
                {
                    viewModel = await _ticketService.GetTicketDashboardCountsByUserId(user.Id);
                }
                else
                {
                    viewModel = await _ticketService.GetTicketDashboardCountsByAdmin();
                }
            }

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
