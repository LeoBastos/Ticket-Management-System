using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Ryze.System.Web.Areas.Admin.Models
{
	public class RegisterFuncionarioViewModel
	{
		[Required]
		[DisplayName("Nome Completo")]
		public string FullName { get; set; }

		[Required]
		[EmailAddress]
		public string? Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string? Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirme a senha")]
		[Compare("Password", ErrorMessage = "As senhas não conferem")]
		public string? ConfirmPassword { get; set; }

		[DisplayName("Cliente?")]
		public bool IsClient { get; set; }
	}
}
