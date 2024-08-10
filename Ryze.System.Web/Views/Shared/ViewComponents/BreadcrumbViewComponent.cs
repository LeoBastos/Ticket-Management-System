using Microsoft.AspNetCore.Mvc;
using Ryze.System.Web.Models;

namespace Ryze.System.Web.Views.Shared.ViewComponents
{
    public class BreadcrumbViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<BreadcrumbItem> items)
        {
            if (items == null)
            {
                items = new List<BreadcrumbItem>();
            }

            return View(items);
        }
    }
}
