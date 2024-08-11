using Microsoft.AspNetCore.Identity;
using Ryze.System.Domain.Entity.Identity;

namespace Ryze.System.Web.Areas.Admin.Models
{
	public class RoleEdit
	{		
		public IdentityRole? Role { get; set; }
		public IEnumerable<ApplicationUser>? Members { get; set; }
		public IEnumerable<ApplicationUser>? NonMembers { get; set; }
	}

}
