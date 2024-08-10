using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Ryze.System.Web.Models.Accounts
{
    public class ChangePasswordViewModel
    {
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Senha Atual")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DisplayName("Nova Senha")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a confirmação não coincidem.")]
        [DisplayName("Confirmar Senha")]
        public string ConfirmPassword { get; set; }
    }
}
