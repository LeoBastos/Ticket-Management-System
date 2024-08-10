using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ryze.System.Application.DTO.Users;
using Ryze.System.Domain.Entity.Identity;

namespace Ryze.System.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserManager<ApplicationUser> _userManager;

        public BaseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected async Task<ApplicationUserDTO> GetUserDetailsAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return new ApplicationUserDTO
                {
                    FullName = "Guest",
                    Avatar = "~/images/noimage.png"
                };
            }

            return new ApplicationUserDTO
            {
                FullName = user.FullName ?? "Guest",
                Avatar = user.Avatar ?? "~/images/noimage.png"
            };
        }
    }
}
