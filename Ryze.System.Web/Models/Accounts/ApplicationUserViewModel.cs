using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace Ryze.System.Web.Models.Accounts
{
    public class ApplicationUserViewModel : IdentityUser
    {
        public bool IsClient { get; set; }
        public bool IsActive { get; set; }
        [DisplayName("Nome Completo")]
        public string? FullName { get; set; }
        public IFormFile? Avatar { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
