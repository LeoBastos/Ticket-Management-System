using System.ComponentModel;

namespace Ryze.System.Web.Models.Accounts
{
    public class EditProfileViewModel
    {
        public EditProfileViewModel()
        {
            
        }
        public string Id { get; set; }
        public string Email { get; set; }

        [DisplayName("Nome Completo")]
        public string FullName { get; set; }

        [DisplayName("Avatar")]
        public IFormFile? Avatar { get; set; }
        public string? UserAvatar { get; set; }
        public bool IsClient { get; set; }
        public bool IsActive { get; set; }

       
    }

}
