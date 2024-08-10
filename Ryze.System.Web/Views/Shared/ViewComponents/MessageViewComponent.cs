using Microsoft.AspNetCore.Mvc;
using Ryze.System.Web.Models;

namespace Ryze.System.Web.Views.Shared.ViewComponents
{
    public class MessageViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string? message = null, string? success = null, string? errors = null)
        {
            var messages = new MessageViewModel
            {
                Message = message,
                Success = success,
                Errors = errors
            };

            return View(messages);
        }
    }
}
