using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ryze.System.Domain.Entity.Identity;
using Ryze.System.Web.Controllers;
using Ryze.System.Web.Models;

namespace Ryze.System.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class AdminController : BaseController
	{
		public AdminController(UserManager<ApplicationUser> userManager) : base(userManager)
		{
		}

		public IActionResult Index()
		{
			var breadcrumbs = new List<BreadcrumbItem>
			{
				new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Home"), IsActive = false },
				new BreadcrumbItem { Text = "Area Administrativa", Url = Url.Action("Index", "Admin"), IsActive = true },
			};

			ViewData["Breadcrumbs"] = breadcrumbs;

			return View();
		}
	}
}
