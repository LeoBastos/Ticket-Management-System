using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ryze.System.Application.DTO.Users;
using Ryze.System.Application.Services.Users;
using Ryze.System.Domain.Entity.Identity;
using Ryze.System.Web.Areas.Admin.Models;
using Ryze.System.Web.Controllers;
using Ryze.System.Web.Models;
using Ryze.System.Web.Models.Accounts;

namespace Ryze.System.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class AdminUsersController : BaseController
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IUserService _userService;


		public AdminUsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
		   RoleManager<IdentityRole> roleManager, IUserService userService) : base(userManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 20, string area = "Admin")
		{
			var breadcrumbs = new List<BreadcrumbItem>
			{
				new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Admin"), IsActive = false },
				new BreadcrumbItem { Text = "Usuários", Url = Url.Action("Index", "AdminUsers"), IsActive = true }
			};

			ViewData["Breadcrumbs"] = breadcrumbs;

			var totalItems = await _userManager.Users.CountAsync(p => p.IsClient == false);
			var users = await _userManager.Users
				.Where(p => p.IsClient == false)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			var userRolesViewModelList = new List<ApplicationUserViewModel>();

			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user);
				var userWithRolesViewModel = new ApplicationUserViewModel
				{
					Id = user.Id,
					FullName = user.FullName,
					Email = user.Email,
					IsActive = user.IsActive,
					Roles = roles.ToList()
				};
				userRolesViewModelList.Add(userWithRolesViewModel);
			}

			var viewModel = new ApplicationUserListViewModel
			{
				Users = userRolesViewModelList,
				PageNumber = pageNumber,
				PageSize = pageSize,
				TotalItems = totalItems,
				Area = area
			};

			
			return View(viewModel);
		}

		[HttpGet]
		public IActionResult Register()
		{
			var breadcrumbs = new List<BreadcrumbItem>
			{
				new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Admin"), IsActive = false },
				new BreadcrumbItem { Text = "Usuários", Url = Url.Action("Index", "AdminUsers"), IsActive = false },
				new BreadcrumbItem { Text = "Cadastrar Usuário", Url = Url.Action("Create", "AdminUsers"), IsActive = true },
			};

			ViewData["Breadcrumbs"] = breadcrumbs;

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterFuncionarioViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser
				{
					FullName = model.FullName,
					UserName = model.Email,
					Email = model.Email,
					IsClient = model.IsClient = false,
					IsActive = true
				};

				var result = await _userManager.CreateAsync(user, model.Password);

				await _userManager.AddToRoleAsync(user, "Funcionario");
				// await _userManager.AddClaimAsync(user, new Claim("IsEmployee", "true"));

				if (result.Succeeded)
				{
                    TempData["Success"] = "Usuário cadastrado com sucesso";
                    return RedirectToAction("Index");
				}

				foreach (var error in result.Errors)
				{
                    TempData["Errors"] = "Erro ao cadastrar novo usuário.";
                    ModelState.AddModelError(string.Empty, error.Description);
				}


			}
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> UpdateUser(string id)
		{
			var breadcrumbs = new List<BreadcrumbItem>
			{
				new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Admin"), IsActive = false },
				new BreadcrumbItem { Text = "Usuários", Url = Url.Action("Index", "AdminUsers"), IsActive = false },
				new BreadcrumbItem { Text = "Editar Usuário", Url = Url.Action("Edit", "AdminUsers"), IsActive = true },
			};

			ViewData["Breadcrumbs"] = breadcrumbs;

			var user = await _userService.GetUserById(id);

			if (user == null)
			{
				ModelState.AddModelError("", "Usuário não encontrado");
				TempData["Errors"] = "Usuário não encontrado.";
				return View("Index");
			}

			ViewBag.IsActive = new List<SelectListItem>
			{
				new SelectListItem { Value = "true", Text = "Ativo" },
				new SelectListItem { Value = "false", Text = "Inativo" }
			};

			return View(user);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> UpdateUser(ApplicationUserDTO userDto)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(userDto.Id);
				if (user == null)
				{
					ModelState.AddModelError("", "Usuário não encontrado");
					TempData["Errors"] = "Usuário não encontrado";
					return RedirectToAction("Index");
				}

				user.FullName = userDto.FullName;
				user.Email = userDto.Email;
				user.IsActive = userDto.IsActive;

				var result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
				{
					TempData["Success"] = "Usuário atualizado com sucesso";
					return RedirectToAction("Index");
				}
			}

			ViewBag.IsActive = new List<SelectListItem>
			{
				new SelectListItem { Value = "true", Text = "Ativo" },
				new SelectListItem { Value = "false", Text = "Inativo" }
			};

			TempData["Errors"] = "Houve um erro ao atualizar o usuário";

			return View(userDto);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteUser(string id)
		{
			var breadcrumbs = new List<BreadcrumbItem>
			{
				new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Admin"), IsActive = false },
				new BreadcrumbItem { Text = "Usuários", Url = Url.Action("Index", "AdminUsers"), IsActive = false },
				new BreadcrumbItem { Text = "Deletar Usuário", Url = Url.Action("Delete", "AdminUsers"), IsActive = true },
			};

			ViewData["Breadcrumbs"] = breadcrumbs;

			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				TempData["Errors"] = "Usuário não encontrado";
				ModelState.AddModelError("", "Usuário não encontrado");
				return View("Index");
			}

			return View(user);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				TempData["Errors"] = "Usuário não encontrado";
				return View("NotFound");
			}
			else
			{
				var result = await _userManager.DeleteAsync(user);

				if (result.Succeeded)
				{
					TempData["Success"] = "Usuário removido.";
					return RedirectToAction("Index");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}

				return View("Index");
			}
		}

		[HttpGet]
		public async Task<IActionResult> AdminChangePassword(string id)
		{
			var breadcrumbs = new List<BreadcrumbItem>
			{
				new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Admin"), IsActive = false },
				new BreadcrumbItem { Text = "Usuários", Url = Url.Action("Index", "AdminUsers"), IsActive = false },
				new BreadcrumbItem { Text = "Trocar Senha", Url = Url.Action("AdminChangePassword", "AdminUsers"), IsActive = true },
			};

			ViewData["Breadcrumbs"] = breadcrumbs;

			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				TempData["Errors"] = "Usuário não encontrado";
				return NotFound();
			}

			var model = new AdminChangePasswordViewModel { UserId = id };
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AdminChangePassword(AdminChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				TempData["Errors"] = "Houve um erro ao acessar.";
				return View(model);
			}

			var user = await _userManager.FindByIdAsync(model.UserId);

			if (user == null)
			{
				return NotFound();
			}

			var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
			var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

			if (result.Succeeded)
			{
				TempData["Success"] = "Senha do usuário alterada com sucesso.";
				return RedirectToAction("Index");
			}
			else
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return View(model);
			}
		}
	}
}
