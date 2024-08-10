using Microsoft.AspNetCore.Mvc;
using Ryze.System.Web.Models;

namespace Ryze.System.Web.Views.Shared.ViewComponents
{
    public class PaginationViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int totalItems, int currentPage, int pageSize, string action, string controller, string? area = "")
        {
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var model = new PaginationViewModel
            {
                CurrentPage = currentPage,
                TotalPages = totalPages,
                PageSize = pageSize,
                Action = action,
                Controller = controller,
                Area = area
            };

            return View(model);
        }
    }
}
