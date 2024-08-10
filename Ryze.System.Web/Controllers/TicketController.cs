using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ryze.System.Application.DTO.Tickets;
using Ryze.System.Application.Services.Tickets;
using Ryze.System.Application.Services.Users;
using Ryze.System.Domain.Entity.Identity;
using Ryze.System.Domain.Enum;
using Ryze.System.Infra.Context;
using Ryze.System.Web.helpers;
using Ryze.System.Web.Models;
using Ryze.System.Web.Models.Tickets;
using System.ComponentModel.DataAnnotations;

namespace Ryze.System.Web.Controllers
{
    public class TicketController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;

        public TicketController(ApplicationDbContext context, IUserService userService,
            ITicketService ticketService, UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            _ticketService = ticketService;
            _userManager = userManager;
            _userService = userService;
        }


        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 20)
        {
            var breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Home"), IsActive = false },
                new BreadcrumbItem { Text = "Tickets", Url = Url.Action("Index", "Ticket"), IsActive = true }
            };

            ViewData["Breadcrumbs"] = breadcrumbs;

            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Unauthorized();

            var viewModel = new TicketListViewModel
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Tickets = new List<TicketViewModel>()
            };

            if (user.IsClient)
            {
                var totalItems = await _ticketService.GetTotalTicketCountByClientId(user.Id);
                var items = await _ticketService.GetPaginatedTicketsByClientId(user.Id, pageNumber, pageSize);

                viewModel.TotalItems = totalItems;

                foreach (var ticket in items)
                {
                    var userName = await _userService.GetUserById(ticket.UserId);

                    viewModel.Tickets.Add(new TicketViewModel
                    {
                        Id = ticket.Id,
                        Description = ticket.Description,
                        ClientImage = user.Avatar,
                        OpeningDate = ticket.OpeningDate,
                        Resolution = ticket.Resolution,
                        UserImage = userName?.Avatar,
                        Status = ticket.Status,
                        Nivel = ticket.Nivel,
                        Priority = ticket.Priority,
                        ClosingDate = ticket.ClosingDate,
                        ClientId = ticket.ClientId,
                        UserId = ticket.UserId,
                        ClientName = user.FullName,
                        UserName = userName?.FullName ?? "Nenhum usuário atribuído"
                    });
                }

                return View(viewModel);
            }
            else
            {
                var totalItems = await _ticketService.GetTotalTicketCount();
                var items = await _ticketService.GetPaginatedTickets(pageNumber, pageSize);

                viewModel.TotalItems = totalItems;

                var users = await _userManager.Users
                    .Where(p => !p.IsClient && p.IsActive)
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id.ToString(),
                        Text = u.UserName
                    })
                    .ToListAsync();

                ViewBag.Encaminhar = new SelectList(users, "Value", "Text");

                foreach (var ticket in items)
                {
                    var clientName = await _userService.GetUserById(ticket.ClientId);
                    var userName = await _userService.GetUserById(ticket.UserId);

                    viewModel.Tickets.Add(new TicketViewModel
                    {
                        Id = ticket.Id,
                        Description = ticket.Description,
                        ClientImage = clientName.Avatar,
                        OpeningDate = ticket.OpeningDate,
                        Resolution = ticket.Resolution,
                        UserImage = userName?.Avatar,
                        Status = ticket.Status,
                        Nivel = ticket.Nivel,
                        Priority = ticket.Priority,
                        ClosingDate = ticket.ClosingDate,
                        ClientId = ticket.ClientId,
                        UserId = ticket.UserId,
                        ClientName = clientName.FullName,
                        UserName = userName?.FullName ?? "Nenhum usuário atribuído"
                    });
                }

                return View(viewModel);
            }
        }

        public async Task<IActionResult> GetTicketsByUserAndStatus(StatusEnum status)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Unauthorized();

            var tickets = await _ticketService.GetTicketsByUserIdAndStatus(user.Id, status);

            var viewModel = new List<TicketDTO>();

            foreach (var ticket in tickets)
            {
                var clientName = await _userService.GetUserById(ticket.ClientId);
                var userName = await _userManager.FindByIdAsync(user.Id);

                viewModel.Add(new TicketDTO
                {
                    Id = ticket.Id,
                    Description = ticket.Description,
                    ClientImage = clientName.Avatar,
                    OpeningDate = ticket.OpeningDate,
                    Resolution = ticket.Resolution,
                    UserImage = userName.Avatar,
                    Status = ticket.Status,
                    Nivel = ticket.Nivel,
                    Priority = ticket.Priority,
                    ClosingDate = ticket.ClosingDate,
                    ClientId = ticket.ClientId,
                    UserId = ticket.UserId,
                    ClientName = clientName?.FullName,
                    UserName = userName?.FullName ?? "Nenhum usuário atribuído"
                });
            }


            return View("TicketByStatus", viewModel);
        }

        public async Task<IActionResult> TicketsByUserAndStatusOpen()
        {
            var breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Home"), IsActive = false },
                new BreadcrumbItem { Text = "Tickets", Url = Url.Action("Index", "Ticket"), IsActive = false },
                new BreadcrumbItem { Text = "Tickets em Aberto", Url = Url.Action("TicketsByUserAndStatusOpen", "Ticket"), IsActive = true }
            };

            ViewData["Breadcrumbs"] = breadcrumbs;

            return await GetTicketsByUserAndStatus(StatusEnum.EmAberto);
        }

        public async Task<IActionResult> TicketsByUserAndStatusInProgress()
        {
            var breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Home"), IsActive = false },
                new BreadcrumbItem { Text = "Tickets", Url = Url.Action("Index", "Ticket"), IsActive = false },
                new BreadcrumbItem { Text = "Tickets em Andamento", Url = Url.Action("TicketsByUserAndStatusInProgress", "Ticket"), IsActive = true }
            };

            ViewData["Breadcrumbs"] = breadcrumbs;

            return await GetTicketsByUserAndStatus(StatusEnum.EmAndamento);
        }

        public async Task<IActionResult> TicketsByUserAndStatusClosed()
        {
            var breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Home"), IsActive = false },
                new BreadcrumbItem { Text = "Tickets", Url = Url.Action("Index", "Ticket"), IsActive = false },
                new BreadcrumbItem { Text = "Tickets em Finalizados", Url = Url.Action("TicketsByUserAndStatusClosed", "Ticket"), IsActive = true }
            };
            ViewData["Breadcrumbs"] = breadcrumbs;

            return await GetTicketsByUserAndStatus(StatusEnum.Fechado);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Home"), IsActive = false },
                new BreadcrumbItem { Text = "Tickets", Url = Url.Action("Index", "Ticket"), IsActive = false },
                new BreadcrumbItem { Text = "Cadastrar Ticket", Url = Url.Action("Create", "Ticket"), IsActive = true }
            };

            ViewData["Breadcrumbs"] = breadcrumbs;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            ViewBag.StatusList = Enum.GetValues(typeof(StatusEnum))
            .Cast<StatusEnum>()
            .Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = GetEnumDisplayName(e)
            }).ToList();

            ViewBag.NivelList = Enum.GetValues(typeof(NivelEnum))
                .Cast<NivelEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = GetEnumDisplayName(e)
                }).ToList();

            ViewBag.PriorityList = Enum.GetValues(typeof(PriorityEnum))
                .Cast<PriorityEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = GetEnumDisplayName(e)
                }).ToList();

            var model = new CreateTicketViewModel
            {
                ClientId = user.Id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                var ticket = new TicketDTO
                {
                    Description = model.Description,
                    ClientImage = await FileHelper.UploadImage(model.ClientImage),
                    OpeningDate = DateTime.Now,
                    ClientId = model.ClientId,
                };

                TempData["Success"] = "Ticket Criado com Sucesso!";

                await _ticketService.Add(ticket);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.StatusList = Enum.GetValues(typeof(StatusEnum))
                .Cast<StatusEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = GetEnumDisplayName(e)
                }).ToList();

            ViewBag.NivelList = Enum.GetValues(typeof(NivelEnum))
                .Cast<NivelEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = GetEnumDisplayName(e)
                }).ToList();

            ViewBag.PriorityList = Enum.GetValues(typeof(PriorityEnum))
                .Cast<PriorityEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = GetEnumDisplayName(e)
                }).ToList();

            TempData["Errors"] = "Ocorreu um erro ao cadastrar o ticket.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Home"), IsActive = false },
                new BreadcrumbItem { Text = "Tickets", Url = Url.Action("Index", "Ticket"), IsActive = false },
                new BreadcrumbItem { Text = "Detalhes do Ticket", Url = Url.Action("Detail", "Ticket"), IsActive = true }
            };

            ViewData["Breadcrumbs"] = breadcrumbs;

            var ticket = await _ticketService.GetTicketById(id);
            var clientName = await _userService.GetUserById(ticket.ClientId);

            if (ticket == null)
            {
                return NotFound();
            }

            var model = new TicketDTO
            {

                Id = ticket.Id,
                Description = ticket.Description,
                ClientName = clientName?.FullName,
                ClientImage = ticket.ClientImage,
                OpeningDate = ticket.OpeningDate,
                Resolution = ticket?.Resolution,
                UserImage = ticket?.UserImage,
                Status = ticket.Status,
                Nivel = ticket.Nivel,
                Priority = ticket.Priority,
                ClosingDate = ticket?.ClosingDate,
                ClientId = ticket?.ClientId,
                UserId = ticket?.UserId,
                UserName = ticket.UserName,
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Home"), IsActive = false },
                new BreadcrumbItem { Text = "Tickets", Url = Url.Action("Index", "Ticket"), IsActive = false },
                new BreadcrumbItem { Text = "Editar Ticket", Url = Url.Action("Edit", "Ticket"), IsActive = true }
            };

            ViewData["Breadcrumbs"] = breadcrumbs;

            var ticket = await _ticketService.GetTicketById(id);
            if (ticket == null || !User.Identity.IsAuthenticated)
            {
                TempData["Errors"] = "Ticket Não Encontrado.";
                return NotFound();
            }

            ViewBag.StatusList = Enum.GetValues(typeof(StatusEnum))
               .Cast<StatusEnum>()
               .Select(e => new SelectListItem
               {
                   Value = e.ToString(),
                   Text = GetEnumDisplayName(e)
               }).ToList();

            ViewBag.NivelList = Enum.GetValues(typeof(NivelEnum))
                .Cast<NivelEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = GetEnumDisplayName(e)
                }).ToList();

            ViewBag.PriorityList = Enum.GetValues(typeof(PriorityEnum))
                .Cast<PriorityEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = GetEnumDisplayName(e)
                }).ToList();


            var model = new EditTicketViewModel
            {
                Id = ticket.Id,
                ClientId = ticket.ClientId,
                OpeningDate = ticket.OpeningDate,
                Description = ticket.Description,
                ClientImageUrl = ticket.ClientImage,
                UserImageUrl = ticket.UserImage,
                Status = ticket.Status,
                Nivel = ticket.Nivel,
                Priority = ticket.Priority,
                Resolution = ticket.Resolution,
                ClosingDate = ticket?.ClosingDate,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ticket = await _ticketService.GetTicketById(model.Id);
                if (ticket == null)
                {
                    TempData["Errors"] = "Ticket Não Encontrado.";
                    return NotFound();
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                ticket.ClientId = model.ClientId;
                ticket.ClientImage = model.ClientImage != null ? FileHelper.UploadImage(model.ClientImage).Result : ticket.ClientImage;
                ticket.Description = model.Description;
                ticket.UserImage = model.UserImage != null ? FileHelper.UploadImage(model.UserImage).Result : ticket.UserImage;
                ticket.Status = model.Status;
                ticket.Nivel = model.Nivel;
                ticket.Priority = model.Priority;
                ticket.Resolution = model.Resolution;
                ticket.UserId = user.Id;

                if (model.Status.ToString() == "Fechado")
                {
                    ticket.ClosingDate = DateTime.Now;
                }

                await _ticketService.Update(ticket);

                TempData["Success"] = "Ticket editado com Sucesso!";

                return RedirectToAction("Index");
            }

            var errors = new List<KeyValuePair<string, string>>();

            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    errors.Add(new KeyValuePair<string, string>(state.Key, error.ErrorMessage));
                }
            }
            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
                TempData["Errors"] = $"Erro ao atualizar o Ticket: {error.Value}";
            }

            TempData["Errors"] = "Ocorreu um erro ao editar o ticket.";

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ForwardingPartial(int id)
        {
            var ticket = await _ticketService.GetTicketById(id);
            if (ticket == null)
            {
                return NotFound();
            }

            var model = new TicketViewModel
            {
                Id = ticket.Id,
                UserId = ticket.UserId
            };

            ViewBag.Encaminhar = new SelectList(await _userManager.GetUsersInRoleAsync("Funcionario"), "Id", "UserName");

            return PartialView("_ForwardingPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Forwarding(TicketViewModel model)
        {

            var ticket = await _ticketService.GetTicketById(model.Id);

            if (ticket == null)
            {
                ViewBag.Encaminhar = new SelectList(await _userManager.GetUsersInRoleAsync("Funcionario"), "Id", "UserName");
                TempData["Errors"] = "Ocorreu um erro encaminhar o ticket.";
                return PartialView("_ForwardingPartial", model);
            }
            var user = await _userService.GetUserById(model.UserId);

            ticket.UserId = model.UserId;
            ticket.UserImage = user.Avatar;

            await _ticketService.UpdatePartial(ticket);

            TempData["Success"] = $"Ticket encaminhado com Sucesso: {ticket.UserName}!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> TakeTicketPartial(int id)
        {
            var ticket = await _ticketService.GetTicketById(id);
            var clientName = await _userService.GetUserById(ticket.ClientId);
            var userData = await _userManager.GetUserAsync(User);

            if (ticket == null)
            {
                return NotFound();
            }

            var model = new TicketViewModel
            {
                Id = ticket.Id,
                UserId = userData.Id,
                ClientName = clientName?.FullName,
                Description = ticket.Description,
                ClientId = ticket.ClientId
            };

            return PartialView("_TakeTicketPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TakeTicket(TicketViewModel model)
        {

            var ticket = await _ticketService.GetTicketById(model.Id);

            if (ticket == null)
            {
                return View(model);
            }
            var user = await _userService.GetUserById(model.UserId);

            ticket.UserId = model.UserId;
            ticket.UserImage = user.Avatar;

            await _ticketService.UpdatePartial(ticket);

            TempData["Success"] = $"Ticket assumido com Sucesso!";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Home"), IsActive = false },
                new BreadcrumbItem { Text = "Tickets", Url = Url.Action("Index", "Ticket"), IsActive = false },
                new BreadcrumbItem { Text = "Deletar Ticket", Url = Url.Action("Delete", "Ticket"), IsActive = true }
            };

            ViewData["Breadcrumbs"] = breadcrumbs;

            var ticket = await _ticketService.GetTicketById(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _ticketService.GetTicketById(id);
            if (ticket == null)
            {
                TempData["Errors"] = "Ticket não encontrado.";

                return NotFound();
            }

            await _ticketService.Remove(id);

            TempData["Success"] = $"Ticket deletado com Sucesso!";

            return RedirectToAction(nameof(Index));
        }

        private string GetEnumDisplayName(Enum enumValue)
        {
            var displayAttribute = enumValue.GetType()
                .GetField(enumValue.ToString())
                .GetCustomAttributes(false)
                .OfType<DisplayAttribute>()
                .FirstOrDefault();

            return displayAttribute != null ? displayAttribute.Name : enumValue.ToString();
        }
    }
}
