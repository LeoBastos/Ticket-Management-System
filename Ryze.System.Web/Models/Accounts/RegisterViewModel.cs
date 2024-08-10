using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Ryze.System.Web.Models.Accounts
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail Inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password é obrigatório")]
        [StringLength(20, ErrorMessage = "O {0} deve ter um minimo de {2} e no máximo " +
            "{1} caracteres.", MinimumLength = 10)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a senha")]
        [Compare("Password", ErrorMessage = "As senhas não conferem")]
        public string? ConfirmPassword { get; set; }

        [DisplayName("Nome Completo")]
        public string? FullName { get; set; }

        [DisplayName("Avatar")]
        public IFormFile? Avatar { get; set; }

        [DisplayName("Cliente?")]
        public bool IsClient { get; set; }

        public bool IsActive { get; set; }
    }
}
