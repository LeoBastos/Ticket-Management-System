using Ryze.System.Application.DTO.Tickets;
using Ryze.System.Domain.Enum;

namespace Ryze.System.Application.Services.Tickets
{
    public interface ITicketService
    {
        Task<int> GetTotalTicketCount();
        Task<List<TicketDTO>> GetPaginatedTickets(int pageNumber, int pageSize);

        Task<int> GetTotalTicketCountByClientId(string clientId);
        Task<List<TicketDTO>> GetPaginatedTicketsByClientId(string clientId, int pageNumber, int pageSize);

        //Task<Dictionary<StatusEnum, int>> GetTicketCountsByUserId(string userId);
        Task<IEnumerable<TicketDTO>> GetTickets();
        Task<IEnumerable<TicketDTO>> GetTicketsInativos();
        Task<IEnumerable<TicketDTO>> GetClosedTickets();
        Task<TicketDTO> GetTicketById(int id);

        Task<List<TicketDTO>> GetTicketsListByUserId(string userId);

        Task<TicketDTO> GetTicketByClientId(string clientId);
        Task<TicketDTO> GetTicketByUserId(string userId);



        Task<IEnumerable<TicketDTO>> GetTicketsByUserIdAndStatus(string userId, StatusEnum status);
        Task<IEnumerable<TicketDTO>> GetTicketsByStatus(string status);
        Task<IEnumerable<TicketDTO>> GetTicketsByPriority(string priority);
        Task<IEnumerable<TicketDTO>> GetTicketsByNivel(string nivel);

        Task Add(TicketDTO ticketDto);
        Task Update(TicketDTO ticketDto);
        Task UpdatePartial(TicketDTO ticketDto);
        Task Remove(int id);
    }
}
