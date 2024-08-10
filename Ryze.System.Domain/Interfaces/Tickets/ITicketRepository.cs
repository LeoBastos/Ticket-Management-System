using Ryze.System.Domain.Entity.Tickets;
using Ryze.System.Domain.Enum;

namespace Ryze.System.Domain.Interfaces.Tickets
{
    public interface ITicketRepository
    {
        #region Querys
        Task<int> GetTotalCountAsync();
        Task<List<Ticket>> GetPaginatedTicketsAsync(int pageNumber, int pageSize);

        //Task<Dictionary<StatusEnum, int>> GetTicketCountsByUserIdAsync(string userId);

        Task<int> GetTotalTicketCountByClientIdAsync(string clientId);
        Task<List<Ticket>> GetPaginatedTicketsByClientIdAsync(string clientId, int pageNumber, int pageSize);



        Task<IEnumerable<Ticket>> GetTicketsAsync();
        Task<IEnumerable<Ticket>> GetTicketsDeactiveAsync();
        Task<IEnumerable<Ticket>> GetClosedTicketsAsync();
        Task<Ticket> GetTicketByIdAsync(int id);
        Task<List<Ticket>> GetTicketsListByUserIdAsync(string usuarioId);

        Task<Ticket> GetTicketByClientIdAsync(string clientId);
        Task<Ticket> GetTicketByUserIdAsync(string userId);

        Task<IEnumerable<Ticket>> GetTicketsByUserIdAndStatus(string userId, StatusEnum status);

        Task<IEnumerable<Ticket>> GetTicketsByStatusAsync(string status);
        Task<IEnumerable<Ticket>> GetTicketsByPriorityAsync(string priority);
        Task<IEnumerable<Ticket>> GetTicketsByNivelAsync(string nivel);

        #endregion

        #region Commands
        Task<Ticket> CreateAsync(Ticket ticket);
        Task<Ticket> UpdateAsync(Ticket ticket);
        Task<Ticket> UpdatePatchAsync(Ticket ticket);
        Task<Ticket> RemoveAsync(Ticket ticket);


        #endregion

    }
}
