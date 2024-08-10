using Ryze.System.Domain.Entity.Identity;

namespace Ryze.System.Domain.Interfaces.Accounts
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetUsersAsync();
        Task<IEnumerable<ApplicationUser>> GetEmployeesAsync();
        Task<IEnumerable<ApplicationUser>> GetClientsAsync();

        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<ApplicationUser> GetUserByNameAsync(string fullName);

        Task<ApplicationUser> CreateAsync(ApplicationUser user);
        Task<ApplicationUser> UpdateAsync(ApplicationUser user);
        Task<ApplicationUser> RemoveAsync(ApplicationUser user);

    }
}
