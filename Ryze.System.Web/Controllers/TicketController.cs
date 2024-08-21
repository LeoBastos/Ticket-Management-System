using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
using System.Reflection;

namespace Ryze.System.Web.Controllers
{
    [Authorize]
    public class TicketController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;
        private readonly IMapper _mapper;

        public TicketController(ApplicationDbContext context, IUserService userService,
            ITicketService ticketService, UserManager<ApplicationUser> userManager, IMapper mapper)
            : base(userManager)
        {
            _ticketService = ticketService;
            _userManager = userManager;
            _userService = userService;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
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
                await PopulateClientTicketsAsync(viewModel, user, pageNumber, pageSize);
            }
            else
            {
                await PopulateOperatorTicketsAsync(viewModel, pageNumber, pageSize);
            }

            return View(viewModel);
        }

        public async Task<IActionResult> GetTicketsByUserAndStatus(StatusEnum status)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Unauthorized();

            if (user.IsClient)
            {
                var tickets = await _ticketService.GetTicketsByClientIdAndStatus(user.Id, status);

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
            else
            {
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
        public async Task<IActionResult> SortTickets(string sortOrder, string sortType)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Unauthorized();

            var tickets = new List<TicketViewModel>();
           
            var items = await _ticketService.GetTickets();

            if (user.IsClient)
            {
                foreach (var ticket in items)
                {
                    var userName = await _userService.GetUserById(ticket.UserId);

                    tickets.Add(new TicketViewModel
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
            }
            else
            {
                foreach (var ticket in items)
                {
                    var clientName = await _userService.GetUserById(ticket.ClientId);
                    var userName = await _userService.GetUserById(ticket.UserId);

                    tickets.Add(new TicketViewModel
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
            }

            
            tickets = sortOrder == "asc" ? sortType switch
            {
                "Status" => tickets.OrderBy(t => t.Status).ToList(),
                "Priority" => tickets.OrderBy(t => t.Priority).ToList(),
                "OpeningDate" => tickets.OrderBy(t => t.OpeningDate).ToList(),
                _ => tickets
            } : sortType switch
            {
                "Status" => tickets.OrderByDescending(t => t.Status).ToList(),
                "Priority" => tickets.OrderByDescending(t => t.Priority).ToList(),
                "OpeningDate" => tickets.OrderByDescending(t => t.OpeningDate).ToList(),
                _ => tickets
            };

            return PartialView("_TicketTablePartial", tickets);
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

            LoadViewBag();

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

            LoadViewBag();

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

            LoadViewBag();


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
                if (user.IsClient)
                {
                    ticket.ClientId = model.ClientId;
                    ticket.ClientImage = model.ClientImage != null ? FileHelper.UploadImage(model.ClientImage).Result : ticket.ClientImage;
                    ticket.Description = model.Description;
                }
                else
                {
                    ticket.ClientId = model.ClientId;
                    ticket.ClientImage = model.ClientImage != null ? FileHelper.UploadImage(model.ClientImage).Result : ticket.ClientImage;
                    ticket.Description = model.Description;
                    ticket.UserImage = model.UserImage != null ? FileHelper.UploadImage(model.UserImage).Result : ticket.UserImage;
                    ticket.Status = model.Status;
                    ticket.Nivel = model.Nivel;
                    ticket.Priority = model.Priority;
                    ticket.Resolution = model.Resolution;
                    ticket.UserId = user.Id;

                }

                if (model.Status.ToString() == "Fechado")
                {
                    ticket.ClosingDate = DateTime.Now;
                }

                await _ticketService.Update(ticket);

                TempData["Success"] = "Ticket editado com Sucesso!";

                return RedirectToAction("Index");
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
            ticket.Status = StatusEnum.EmAndamento;

            await _ticketService.Update(ticket);

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
                ClientId = ticket.ClientId,               
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

            await _ticketService.Update(ticket);

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
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var ticket = await _ticketService.GetTicketById(id);
            if (ticket == null)
            {
                return Json(new { success = false, message = "Ticket não encontrado." });
            }

            await _ticketService.Remove(id);

            TicketCountsResult ticketCounts = null;

            if (User.IsInRole("Admin") || User.IsInRole("Gerente"))
            {
                ticketCounts = await _ticketService.GetTicketDashboardCountsByAdmin();
            }
            else if (User.IsInRole("Funcionario"))
            {
                ticketCounts = await _ticketService.GetTicketDashboardCountsByUserId(user.Id);
            }
            else if (User.IsInRole("Cliente"))
            {
                ticketCounts = await _ticketService.GetTicketDashboardCountsByClientId(user.Id);
            }

            return Json(new { success = true, message = "Ticket deletado com sucesso!", ticketCounts });
        }


        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Unauthorized();
            
            var (tickets, clientAvatars) = await _ticketService.GetTicketsBySearchTerm(searchTerm, pageNumber, pageSize);

            var viewModel = new TicketListViewModel
            {
                SearchTerm = searchTerm,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Tickets = tickets.Select(ticket => new TicketViewModel
                {
                    Id = ticket.Id,
                    Description = ticket.Description,
                    ClientImage = clientAvatars.TryGetValue(ticket.ClientId, out var avatarUrl) ? avatarUrl : string.Empty,
                    OpeningDate = ticket.OpeningDate,
                    Resolution = ticket.Resolution,
                    UserImage = ticket.User?.Avatar,
                    Status = ticket.Status,
                    Nivel = ticket.Nivel,
                    Priority = ticket.Priority,
                    ClosingDate = ticket.ClosingDate,
                    ClientId = ticket.ClientId,
                    UserId = ticket.UserId,
                    ClientName = ticket.Client?.FullName,
                    UserName = ticket.User?.FullName ?? "Nenhum usuário atribuído"
                }).ToList()
            };

            return View("Index", viewModel);
        }


        #region Private

        private async Task PopulateClientTicketsAsync(TicketListViewModel viewModel, ApplicationUser user, int pageNumber, int pageSize)
        {
            viewModel.TotalItems = await _ticketService.GetTotalTicketCountByClientId(user.Id);
            var tickets = await _ticketService.GetPaginatedTicketsByClientId(user.Id, pageNumber, pageSize);

            foreach (var ticket in tickets)
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
        }

        private async Task PopulateOperatorTicketsAsync(TicketListViewModel viewModel, int pageNumber, int pageSize)
        {
            viewModel.TotalItems = await _ticketService.GetTotalTicketCount();
            var tickets = await _ticketService.GetPaginatedTickets(pageNumber, pageSize);

            var users = await _userManager.Users
                .Where(p => !p.IsClient && p.IsActive)
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.UserName
                })
                .ToListAsync();

            ViewBag.Encaminhar = new SelectList(users, "Value", "Text");

            foreach (var ticket in tickets)
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
        }

      
        private void LoadViewBag()
        {
            ViewBag.StatusList = GetEnumSelectList<StatusEnum>();
            ViewBag.NivelList = GetEnumSelectList<NivelEnum>();
            ViewBag.PriorityList = GetEnumSelectList<PriorityEnum>();
        }

        private List<SelectListItem> GetEnumSelectList<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = GetEnumDisplayName(e)
                })
                .ToList();
        }

        private string GetEnumDisplayName(Enum value)
        {           
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttribute<DisplayAttribute>();
            return attribute != null ? attribute.Name : value.ToString();
        }

        #endregion
    }
}
