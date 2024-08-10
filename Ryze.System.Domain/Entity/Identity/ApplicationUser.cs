using Microsoft.AspNetCore.Identity;

namespace Ryze.System.Domain.Entity.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsClient { get; set; }
        public bool IsActive { get; set; } = true;
        public string? FullName { get; set; }
        public string? Avatar { get; set; }


        public void UpdateName(string fullName)
        {
            FullName = fullName;
        }

        public void UpdateAvatar(string avatar)
        {
            Avatar = avatar;
        }

        public void Remove()
        {
            IsActive = false;
        }
    }
}
