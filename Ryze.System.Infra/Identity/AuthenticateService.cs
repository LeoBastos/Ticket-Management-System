using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Ryze.System.Domain.Entity.Identity;
using Ryze.System.Domain.Interfaces.Accounts;
using System.Web;

namespace Ryze.System.Infra.Identity
{
    public class AuthenticateService : IAuthenticate
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;


        public AuthenticateService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                                    IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<AuthenticationResult> Authenticate(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthenticationResult { Success = false, ErrorMessage = "Usuário não encontrado." };
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return new AuthenticationResult { Success = true };
            }

            if (result.IsLockedOut)
            {
                return new AuthenticationResult { Success = false, ErrorMessage = "Conta bloqueada." };
            }

            return new AuthenticationResult { Success = false, ErrorMessage = "Credenciais inválidas." };
        }

        public async Task<RegisterResult> RegisterUserAsync(string email, string password, bool isClient)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                IsClient = isClient
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {

                return new RegisterResult { Success = true };
            }
            else
            {
                return new RegisterResult { Success = false, Errors = result.Errors.Select(e => e.Code) };
            }
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetUrl = $"{_configuration["FrontendUrl"]}/reset-password?token={HttpUtility.UrlEncode(token)}&email={HttpUtility.UrlEncode(email)}";

            var message = $"Clique <a href='{resetUrl}'>aqui</a> para redefinir sua senha.";
                      
            return token;
        }

        public async Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> AddRoleToUserAsync(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            return await _userManager.AddToRoleAsync(user, role);
        }

    }
}
