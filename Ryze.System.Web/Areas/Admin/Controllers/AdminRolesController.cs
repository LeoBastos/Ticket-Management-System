using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ryze.System.Domain.Entity.Identity;
using Ryze.System.Web.Areas.Admin.Models;
using Ryze.System.Web.Controllers;
using Ryze.System.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace Ryze.System.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class AdminRolesController : BaseController
	{
		private RoleManager<IdentityRole> roleManager;
		private UserManager<ApplicationUser> userManager;

		public AdminRolesController(RoleManager<IdentityRole> roleManager,
			UserManager<ApplicationUser> userManager) : base(userManager)
		{
			this.roleManager = roleManager;
			this.userManager = userManager;
		}

		public ViewResult Index()
		{
			var breadcrumbs = new List<BreadcrumbItem>
			{
				new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Admin"), IsActive = false },
				new BreadcrumbItem { Text = "Roles", Url = Url.Action("Index", "AdminRoles"), IsActive = true },
			};

			ViewData["Breadcrumbs"] = breadcrumbs;

			return View(roleManager.Roles);
		}

		public IActionResult Create()
		{
			var breadcrumbs = new List<BreadcrumbItem>
			{
				new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Admin"), IsActive = false },
				new BreadcrumbItem { Text = "Roles", Url = Url.Action("Index", "AdminRoles"), IsActive = false },
				new BreadcrumbItem { Text = "Criar Role", Url = Url.Action("Create", "AdminRoles"), IsActive = true },
			};

			ViewData["Breadcrumbs"] = breadcrumbs;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create([Required] string name)
		{
			if (ModelState.IsValid)
			{
				IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
				if (result.Succeeded)
				{
					TempData["Message"] = "Role criada com sucesso.";
					return RedirectToAction("Index");
				}
				else
				{
					TempData["Errors"] = "Erro ao criar a Role.";
					Errors(result);
				}

			}
			return View(name);
		}

		[HttpGet]
		public async Task<IActionResult> Update(string id)
		{
			var breadcrumbs = new List<BreadcrumbItem>
			{
				new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Admin"), IsActive = false },
				new BreadcrumbItem { Text = "Roles", Url = Url.Action("Index", "AdminRoles"), IsActive = false },
				new BreadcrumbItem { Text = "Editar Role", Url = Url.Action("Edit", "AdminRoles"), IsActive = true },
			};

			ViewData["Breadcrumbs"] = breadcrumbs;

			IdentityRole role = await roleManager.FindByIdAsync(id);

			List<ApplicationUser> members = new List<ApplicationUser>();
			List<ApplicationUser> nonMembers = new List<ApplicationUser>();

			var users = await userManager.Users.ToListAsync();

			foreach (ApplicationUser user in users)
			{
				var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;

				list.Add(user);
			}

			return View(new RoleEdit
			{
				Role = role,
				Members = members,
				NonMembers = nonMembers
			});

		}

		[HttpPost]
		public async Task<IActionResult> Update(RoleModification model)
		{
			IdentityResult result;
			if (ModelState.IsValid)
			{
				foreach (string userId in model.AddIds ?? new string[] { })
				{
					ApplicationUser user = await userManager.FindByIdAsync(userId);

					if (user != null && !await userManager.IsInRoleAsync(user, model.RoleName))
					{
						result = await userManager.AddToRoleAsync(user, model.RoleName);
						if (!result.Succeeded) Errors(result);
					}
				}
				foreach (string userId in model.DeleteIds ?? new string[] { })
				{
					ApplicationUser user = await userManager.FindByIdAsync(userId);

					if (user != null && await userManager.IsInRoleAsync(user, model.RoleName))
					{
						result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
						if (!result.Succeeded) Errors(result);
					}
				}
			}
			if (ModelState.IsValid)
			{
				TempData["Message"] = "Atualizado com sucesso.";
				return RedirectToAction(nameof(Index));
			}
			else
			{
				TempData["Errors"] = "Erro ao atualizar a role do usuário.";
				return await Update(model.RoleId);
			}
		}


		[HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			var breadcrumbs = new List<BreadcrumbItem>
			{
				new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Admin"), IsActive = false },
				new BreadcrumbItem { Text = "Roles", Url = Url.Action("Index", "AdminRoles"), IsActive = false },
				new BreadcrumbItem { Text = "Deletar Role", Url = Url.Action("Delete", "AdminRoles"), IsActive = true },
			};

			ViewData["Breadcrumbs"] = breadcrumbs;

			IdentityRole role = await roleManager.FindByIdAsync(id);

			if (role == null)
			{
				ModelState.AddModelError("", "Role não encontrada");
				return View("Index", roleManager.Roles);
			}

			return View(role);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var role = await roleManager.FindByIdAsync(id);
			if (role != null)
			{
				IdentityResult result = await roleManager.DeleteAsync(role);

				if (result.Succeeded)
				{
					TempData["Message"] = "Role excluida com sucesso.";
					return RedirectToAction("Index");
				}

				else
				{
					TempData["Errors"] = "Erro ao excluir a role.";
					Errors(result);
				}
			}
			else
			{
				TempData["Errors"] = "Role não encontrada.";
				ModelState.AddModelError("", "Role Não Encontrada");
			}

			return View("Index", roleManager.Roles);
		}

		private void Errors(IdentityResult result)
		{
			foreach (IdentityError error in result.Errors)
				ModelState.AddModelError("", error.Description);
		}

	}
}
