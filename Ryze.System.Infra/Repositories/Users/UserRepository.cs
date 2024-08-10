using Microsoft.EntityFrameworkCore;
using Ryze.System.Domain.Entity.Identity;
using Ryze.System.Domain.Interfaces.Accounts;
using Ryze.System.Infra.Context;

namespace Ryze.System.Infra.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _userContext;

        public UserRepository(ApplicationDbContext context)
        {
            _userContext = context;
        }

        #region Querys
        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            return await _userContext.Users.ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetEmployeesAsync()
        {
            return await _userContext.Users.Where(p => p.IsClient == false).ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetClientsAsync()
        {
            return await _userContext.Users.Where(p => p.IsClient == true).ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _userContext.Users.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<ApplicationUser> GetUserByNameAsync(string fullName)
        {
            return await _userContext.Users.FirstOrDefaultAsync(t => t.FullName == fullName);
        }

        #endregion

        #region Commands
        public async Task<ApplicationUser> CreateAsync(ApplicationUser user)
        {
            _userContext.Add(user);

            await _userContext.SaveChangesAsync();

            return user;
        }

        public async Task<ApplicationUser> UpdateAsync(ApplicationUser user)
        {
            try
            {
                var existingUser = await _userContext.Users.FindAsync(user.Id);
                if (existingUser != null)
                {
                    _userContext.Entry(existingUser).State = EntityState.Detached;
                }

                _userContext.Update(user);
                await _userContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return user;

        }

        public async Task<ApplicationUser> RemoveAsync(ApplicationUser user)
        {
            _userContext.Remove(user);

            await _userContext.SaveChangesAsync();

            return user;
        }
        #endregion
    }
}
