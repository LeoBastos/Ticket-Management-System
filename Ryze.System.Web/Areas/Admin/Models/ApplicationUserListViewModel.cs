using Ryze.System.Web.Models.Accounts;

namespace Ryze.System.Web.Areas.Admin.Models
{
	public class ApplicationUserListViewModel
	{
		public List<ApplicationUserViewModel> Users { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public int TotalItems { get; set; }
		public int CurrentPage { get; set; }
		public string? Area { get; set; }
	}
}
