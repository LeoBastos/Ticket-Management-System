using System.ComponentModel;

namespace Ryze.System.Application.DTO.Users
{
    public class ApplicationUserDTO
    {
        public ApplicationUserDTO()
        {

        }

        public ApplicationUserDTO(string? id, string email, bool isClient, bool isActive)
        {
            Id = id;
            Email = email;
            IsClient = isClient;
            IsActive = isActive;
        }

        public ApplicationUserDTO(string? id, string email, bool isClient, bool isActive, string? fullName, string? avatar, List<string> roles)
        {
            Id = id;
            Email = email ?? throw new ArgumentNullException(nameof(email));
            IsClient = isClient;
            IsActive = isActive;
            FullName = fullName;
            Avatar = avatar;
            Roles = roles ?? throw new ArgumentNullException(nameof(roles));
        }

        public string? Id { get; set; }
        public string Email { get; set; }

        [DisplayName("Cliente?")]
        public bool IsClient { get; set; }

        [DisplayName("Ativo?")]
        public bool IsActive { get; set; }
        [DisplayName("Nome Completo")]
        public string? FullName { get; set; }
        public string? Avatar { get; set; }

        public string? UserName { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? NormalizedEmail { get; set; }

        public List<string> Roles { get; set; } = new List<string>();
    }
}
