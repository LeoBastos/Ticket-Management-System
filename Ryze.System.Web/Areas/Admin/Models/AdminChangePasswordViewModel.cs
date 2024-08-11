using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Ryze.System.Web.Areas.Admin.Models
{
	public class AdminChangePasswordViewModel
	{
		public string UserId { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
		[DisplayName("Password")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "A nova senha e a confirmação não coincidem.")]
		[DisplayName("Confirmar Password")]
		public string ConfirmPassword { get; set; }
	}
}
