using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace Ryze.System.Web.Areas.Admin.Models
{
	public class ApplicationUserAreaViewModel : IdentityUser
	{
		public bool IsClient { get; set; }
		[DisplayName("Ativo?")]
		public bool IsActive { get; set; }
		[DisplayName("Nome Completo")]
		public string? FullName { get; set; }

		public IFormFile Avatar { get; set; }
		public List<string> Roles { get; set; } = new List<string>();
	}
}
