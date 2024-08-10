using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ryze.System.Application.Services.Users;
using Ryze.System.Domain.Entity.Identity;

namespace Ryze.System.Web.Views.Shared.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;

        public NavbarViewComponent(UserManager<ApplicationUser> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);           
            var userData = await _userService.GetUserById(user.Id);

            return View(userData);
        }
    }

}
