using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Ryze.System.Application.DTO.Users;
using Ryze.System.Domain.Entity.Identity;
using Ryze.System.Domain.Interfaces.Accounts;

namespace Ryze.System.Application.Services.Users
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper, IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUserDTO>> GetUsers()
        {
            var result = await _userRepository.GetUsersAsync();
            return _mapper.Map<IEnumerable<ApplicationUserDTO>>(result);
        }

        public async Task<IEnumerable<ApplicationUserDTO>> GetEmployees()
        {
            var result = await _userRepository.GetEmployeesAsync();
            return _mapper.Map<IEnumerable<ApplicationUserDTO>>(result);
        }

        public async Task<IEnumerable<ApplicationUserDTO>> GetClients()
        {
            var result = await _userRepository.GetClientsAsync();
            return _mapper.Map<IEnumerable<ApplicationUserDTO>>(result);
        }

        public async Task<ApplicationUserDTO> GetUserById(string id)
        {
            var result = await _userRepository.GetUserByIdAsync(id);
            if (result == null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(result);
            var userDto = _mapper.Map<ApplicationUserDTO>(result);
            userDto.Roles = roles.ToList();

            return userDto;
        }

        public async Task<ApplicationUserDTO> GetUserByName(string fullName)
        {
            var result = await _userRepository.GetUserByNameAsync(fullName);

            return _mapper.Map<ApplicationUserDTO>(result);
        }

        public async Task Create(ApplicationUserDTO userDto)
        {
            try
            {
                var result = _mapper.Map<ApplicationUser>(userDto);
                await _userRepository.CreateAsync(result);
            }
            catch (Exception e)
            {
                throw new Exception("Description: " + e);
            }
        }

        public async Task Update(ApplicationUserDTO userDto)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userDto.Id);

                if (user == null)
                {
                    throw new Exception("Usuário não encontrado");
                }

                _mapper.Map(userDto, user);

                await _userRepository.UpdateAsync(user);
            }
            catch (Exception e)
            {
                throw new Exception("Description: " + e);
            }
        }

        public async Task Remove(string id)
        {
            var entity = _userRepository.GetUserByIdAsync(id).Result;
            entity.Remove();
            await _userRepository.UpdateAsync(entity);
        }



        public async Task UpdateName(ApplicationUserDTO userDto)
        {
            var user = await _userRepository.GetUserByNameAsync(userDto.FullName);
            var result = _mapper.Map<ApplicationUser>(userDto);

            await _userRepository.UpdateAsync(result);
        }

        public async Task UpdateAvatar(ApplicationUserDTO userDto)
        {
            var user = await _userRepository.GetUserByNameAsync(userDto.Avatar);
            var result = _mapper.Map<ApplicationUser>(userDto);

            await _userRepository.UpdateAsync(result);
        }
    }
}
