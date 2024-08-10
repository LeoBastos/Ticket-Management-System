using Ryze.System.Application.DTO.Users;

namespace Ryze.System.Application.Services.Users
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUserDTO>> GetUsers();
        Task<ApplicationUserDTO> GetUserById(string id);

        Task<ApplicationUserDTO> GetUserByName(string fullName);

        Task<IEnumerable<ApplicationUserDTO>> GetEmployees();
        Task<IEnumerable<ApplicationUserDTO>> GetClients();

        Task Create(ApplicationUserDTO userDto);
        Task Update(ApplicationUserDTO userDto);
        Task UpdateName(ApplicationUserDTO userDto);
        Task UpdateAvatar(ApplicationUserDTO userDto);
        Task Remove(string id);
    }
}
