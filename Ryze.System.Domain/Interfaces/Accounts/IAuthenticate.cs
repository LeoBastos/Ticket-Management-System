using Microsoft.AspNetCore.Identity;
using Ryze.System.Domain.Entity.Identity;

namespace Ryze.System.Domain.Interfaces.Accounts
{
    public interface IAuthenticate
    {
        Task<AuthenticationResult> Authenticate(string email, string password);
        Task<RegisterResult> RegisterUserAsync(string email, string password, bool isCliente);
        Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword);
        Task<IdentityResult> AddRoleToUserAsync(string email, string role);

        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task Logout();
    }
}
