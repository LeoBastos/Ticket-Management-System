using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Ryze.System.Application.DTO.Users;
using Ryze.System.Application.Services.Users;
using Ryze.System.Domain.Entity.Identity;
using Ryze.System.Web.helpers;
using Ryze.System.Web.Models;
using Ryze.System.Web.Models.Accounts;

namespace Ryze.System.Web.Controllers
{

    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IUserService userService, IEmailSender emailSender) : base(userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    Avatar = Url.Content("~/images/noimage.png"),
                    IsClient = true,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                await _userManager.AddToRoleAsync(user, "Cliente");

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewData["title"] = "Ryzem System";
            return View();
        }

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Usuário não encontrado.");
                    return View(model);
                }
                if (!user.IsActive)
                {
                    await _signInManager.SignOutAsync();
                    ModelState.AddModelError(string.Empty, "Sua conta está desativada.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {                  
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Login Inválido");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Home"), IsActive = false },
                new BreadcrumbItem { Text = "Conta", Url = Url.Action("Index", "Account"), IsActive = false },
                new BreadcrumbItem { Text = "Editar Conta", Url = Url.Action("Edit", "Account"), IsActive = true }
            };

            ViewData["Breadcrumbs"] = breadcrumbs;

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var model = new EditProfileViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                UserAvatar = user.Avatar
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);

                if (user == null)
                {
                    return NotFound();
                }

                var result = new ApplicationUserDTO
                {
                    Id = model.Id,
                    FullName = model.FullName,
                    Email = model.Email,
                    Avatar = model.Avatar != null ? await FileHelper.UploadImage(model.Avatar) : user.Avatar,
                    IsClient = user.IsClient,
                    IsActive = user.IsActive,
                    UserName = model.Email,
                    NormalizedEmail = model.Email,
                    NormalizedUserName = model.Email
                };

                await _userService.Update(result);

                TempData["Success"] = "Perfil atualizado com sucesso";

                return View(model);
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            var breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Home"), IsActive = false },
                new BreadcrumbItem { Text = "Conta", Url = Url.Action("Index", "Account"), IsActive = false },
                new BreadcrumbItem { Text = "Trocar Password", Url = Url.Action("ChangePassword", "Account"), IsActive = true }
            };

            ViewData["Breadcrumbs"] = breadcrumbs;

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ChangePasswordViewModel
            {
                UserId = user.Id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errors"] = "Houve um erro ao trocar sua senha.";
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                TempData["Errors"] = "Usuário não encontrado.";
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["Success"] = "Senha alterada com sucesso.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                TempData["Errors"] = "Houve um erro ao trocar sua senha.";
                return View(model);
            }
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            ViewData["title"] = "Ryzem System";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return RedirectToAction("ForgotPassword");
                }

                var tokenUser = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(nameof(ResetPassword), "Account", new { userId = user.Id, token = tokenUser }, protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(email, "Resetar Senha",
                    $"Por favor, resete sua senha clicando <a href='{callbackUrl}'>aqui</a>.");

                return RedirectToAction("ForgotPasswordConfirmation");
            }
            catch (Exception ex)
            {
                TempData["Errors"] = $"Houve um erro ao trocar sua senha. {ex.Message}";
                throw new Exception(ex.Message); // Console.WriteLine(ex.Message);
            }

        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("Invalid reset token.");
            }

            var model = new ResetPasswordViewModel
            {
                UserId = userId,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return RedirectToAction("ResetPassword");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [Route("/Account/AccessDenied")]
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
