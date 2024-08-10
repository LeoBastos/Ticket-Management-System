using Microsoft.EntityFrameworkCore;
using Ryze.System.Domain.Entity.Tickets;
using Ryze.System.Domain.Enum;
using Ryze.System.Domain.Interfaces.Tickets;
using Ryze.System.Infra.Context;

namespace Ryze.System.Infra.Repositories.Tickets
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _ticketContext;

        public TicketRepository(ApplicationDbContext context)
        {
            _ticketContext = context;
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

        //public async Task<Dictionary<StatusEnum, int>> GetTicketCountsByUserIdAsync(string userId)
        //{
        //    var tickets = await _ticketContext.Tickets
        //        .Where(t => t.UserId == userId)
        //        .GroupBy(t => t.Status)
        //        .Select(g => new { Status = g.Key, Count = g.Count() })
        //        .ToListAsync();

        //    var ticketCounts = tickets.ToDictionary(t => t.Status, t => t.Count);

        //    foreach (StatusEnum status in Enum.GetValues(typeof(StatusEnum)))
        //    {
        //        if (!ticketCounts.ContainsKey(status))
        //        {
        //            ticketCounts[status] = 0;
        //        }
        //    }

        //    return ticketCounts;
        //}

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


        public async Task<IEnumerable<Ticket>> GetTicketsByUserIdAndStatus(string userId, StatusEnum status)
        {
            return await _ticketContext.Tickets
                .Where(p => p.IsActive == true)
                .Where(t => t.UserId == userId && t.Status == status)
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

            //var existingTicket = await _ticketContext.Tickets.FindAsync(ticket.Id);
            //if (existingTicket != null)
            //{
            //    _ticketContext.Entry(existingTicket).State = EntityState.Detached;
            //}

            //_ticketContext.Update(ticket);
            //await _ticketContext.SaveChangesAsync();

            //return ticket;
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
