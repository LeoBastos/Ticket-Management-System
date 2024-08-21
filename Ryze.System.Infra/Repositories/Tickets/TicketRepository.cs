using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ryze.System.Domain.Entity.Identity;
using Ryze.System.Domain.Entity.Tickets;
using Ryze.System.Domain.Enum;
using Ryze.System.Domain.Interfaces.Tickets;
using Ryze.System.Infra.Context;

namespace Ryze.System.Infra.Repositories.Tickets
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _ticketContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _ticketContext = context;
            _userManager = userManager;
        }

        #region Querys   

        // Paginate para Funcionários
        public async Task<int> GetTotalCountAsync()
        {
            return await _ticketContext.Tickets.Where(p => p.IsActive == true).CountAsync();
        }

        // Paginate para Funcionários
        public async Task<List<Ticket>> GetPaginatedTicketsAsync(int pageNumber, int pageSize)
        {
            return await _ticketContext.Tickets
                .Where(t => t.IsActive == true)
                .OrderByDescending(t => t.OpeningDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // Paginate para Cliente
        public async Task<int> GetTotalTicketCountByClientIdAsync(string clientId)
        {
            return await _ticketContext.Tickets.Where(p => p.IsActive == true).CountAsync(t => t.ClientId == clientId);
        }

        // Paginate para Cliente
        public async Task<List<Ticket>> GetPaginatedTicketsByClientIdAsync(string clientId, int pageNumber, int pageSize)
        {
            return await _ticketContext.Tickets
                .Where(t => t.ClientId == clientId)
                .Where(t => t.IsActive == true)
                .OrderByDescending(t => t.OpeningDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        //dashboard
        public async Task<Dictionary<StatusEnum, int>> GetTicketDashboardCountsByAdminAsync()
        {
            var tickets = await _ticketContext.Tickets
               .Where(t => t.IsActive == true)
               .GroupBy(t => t.Status)
               .Select(g => new { Status = g.Key, Count = g.Count() })
               .ToListAsync();

            var ticketCounts = tickets.ToDictionary(t => t.Status, t => t.Count);

            foreach (StatusEnum status in Enum.GetValues(typeof(StatusEnum)))
            {
                if (!ticketCounts.ContainsKey(status))
                {
                    ticketCounts[status] = 0;
                }
            }

            return ticketCounts;
        }

        //dashboard
        public async Task<Dictionary<StatusEnum, int>> GetTicketDashboardCountsByClientIdAsync(string userId)
        {
            var tickets = await _ticketContext.Tickets
                .Where(t => t.ClientId == userId)
                .Where(s => s.IsActive == true)
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            var ticketCounts = tickets.ToDictionary(t => t.Status, t => t.Count);

            foreach (StatusEnum status in Enum.GetValues(typeof(StatusEnum)))
            {
                if (!ticketCounts.ContainsKey(status))
                {
                    ticketCounts[status] = 0;
                }
            }

            return ticketCounts;
        }
        //dashboard
        public async Task<Dictionary<StatusEnum, int>> GetTicketDashboardCountsByUserIdAsync(string userId)
        {
            var tickets = await _ticketContext.Tickets
                .Where(t => t.UserId == userId)
                .Where(t => t.IsActive == true)
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            var ticketCounts = tickets.ToDictionary(t => t.Status, t => t.Count);

            foreach (StatusEnum status in Enum.GetValues(typeof(StatusEnum)))
            {
                if (!ticketCounts.ContainsKey(status))
                {
                    ticketCounts[status] = 0;
                }
            }

            return ticketCounts;
        }

        public async Task<Dictionary<StatusEnum, int>> GetTicketCountsByUserIdAsync(string userId)
        {
            var tickets = await _ticketContext.Tickets
                .Where(t => t.UserId == userId)
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            var ticketCounts = tickets.ToDictionary(t => t.Status, t => t.Count);

            foreach (StatusEnum status in Enum.GetValues(typeof(StatusEnum)))
            {
                if (!ticketCounts.ContainsKey(status))
                {
                    ticketCounts[status] = 0;
                }
            }

            return ticketCounts;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync()
        {

            return await _ticketContext.Tickets
                .Where(p => p.IsActive == true)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsDeactiveAsync()
        {
            return await _ticketContext.Tickets.Where(p => p.IsActive == false).ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetClosedTicketsAsync()
        {
            return await _ticketContext.Tickets.Where(p => p.Status.ToString() == "Fechado").ToListAsync();
        }

        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            return await _ticketContext.Tickets
                    .Include(t => t.Client)
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Ticket>> GetTicketsListByUserIdAsync(string userId)
        {
            return await _ticketContext.Tickets
                    .Where(t => t.ClientId == userId || t.UserId == userId)
                    .Where(p => p.IsActive == true)
                    .ToListAsync();
        }


        public async Task<Ticket> GetTicketByClientIdAsync(string clientId)
        {
            return await _ticketContext.Tickets
                .Where(p => p.IsActive == true)
                .FirstOrDefaultAsync(t => t.ClientId == clientId);
        }

        public async Task<Ticket> GetTicketByUserIdAsync(string userId)
        {
            return await _ticketContext.Tickets
                .Where(p => p.IsActive == true)
                .FirstOrDefaultAsync(t => t.UserId == userId);
        }


        //return tickets por status por usuário
        public async Task<IEnumerable<Ticket>> GetTicketsByUserIdAndStatusAsync(string userId, StatusEnum status)
        {
            return await _ticketContext.Tickets
                .Where(p => p.IsActive == true)
                .Where(t => t.UserId == userId && t.Status == status)
                .ToListAsync();
        }

        //return tickets por status por cliente
        public async Task<IEnumerable<Ticket>> GetTicketsByClientIdAndStatusAsync(string userId, StatusEnum status)
        {
            return await _ticketContext.Tickets
                .Where(p => p.IsActive == true)
                .Where(t => t.ClientId == userId && t.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByNivelAsync(string nivel)
        {

            return await _ticketContext.Tickets
                .Where(p => p.IsActive == true && p.Nivel.ToString() == nivel)
                .ToListAsync();

        }

        public async Task<IEnumerable<Ticket>> GetTicketsByPriorityAsync(string priority)
        {

            return await _ticketContext.Tickets
                    .Where(p => p.IsActive == true && p.Priority.ToString() == priority)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByStatusAsync(string status)
        {
            return await _ticketContext.Tickets
                    .Where(p => p.IsActive == true && p.Status.ToString() == status)
                    .ToListAsync();
        }

        //retorna qtd de paginas para paginação da pesquisa
        public async Task<int> GetTotalTicketCountBySearchTermAsync(string searchTerm)
        {
            return await _ticketContext.Tickets
                .Where(t => t.Client.FullName.Contains(searchTerm) || t.User.FullName.Contains(searchTerm))
                .CountAsync();
        }
        //retorna resultado da pesquisa       
        public async Task<(IList<Ticket> Tickets, Dictionary<string, string> ClientAvatars)> GetTicketsBySearchTermAsync(
                            string searchTerm, int pageNumber, int pageSize)
        {
            var tickets = await _ticketContext.Tickets
                .Where(t => t.Client.FullName.Contains(searchTerm) || t.User.FullName.Contains(searchTerm))
                .OrderBy(t => t.OpeningDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var clientIds = tickets.Select(t => t.ClientId).Distinct().ToList();
            var clients = await _userManager.Users
                .Where(u => clientIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.Avatar);

            return (tickets, clients);
        }


        #endregion

        #region Commands
        public async Task<Ticket> CreateAsync(Ticket ticket)
        {
            _ticketContext.Add(ticket);

            await _ticketContext.SaveChangesAsync();
            return ticket;
        }

        public async Task<Ticket> UpdateAsync(Ticket ticket)
        {
            var existingTicket = await _ticketContext.Tickets.FindAsync(ticket.Id);
            if (existingTicket != null)
            {
                _ticketContext.Entry(existingTicket).State = EntityState.Detached;
            }

            _ticketContext.Update(ticket);
            await _ticketContext.SaveChangesAsync();

            return ticket;
        }

        public async Task<Ticket> UpdatePatchAsync(Ticket ticket)
        {
            var existingTicket = await _ticketContext.Tickets.FindAsync(ticket.Id);
            if (existingTicket != null)
            {
                existingTicket.PatchUpdate(ticket.UserId);
                _ticketContext.Tickets.Update(existingTicket);
                await _ticketContext.SaveChangesAsync();
            }

            return existingTicket;
        }

        public async Task<Ticket> RemoveAsync(Ticket ticket)
        {
            _ticketContext.Remove(ticket);
            await _ticketContext.SaveChangesAsync();
            return ticket;
        }
        #endregion
    }
}
