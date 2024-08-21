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


        //dashboard
        Task<TicketCountsResult> GetTicketDashboardCountsByAdmin();

        //dashboard
        Task<TicketCountsResult> GetTicketDashboardCountsByClientId(string clientId);
        //dashboard
        Task<TicketCountsResult> GetTicketDashboardCountsByUserId(string clientId);

        Task<Dictionary<StatusEnum, int>> GetTicketCountsByUserId(string userId);

        Task<IEnumerable<TicketDTO>> GetTickets();
        Task<IEnumerable<TicketDTO>> GetTicketsInativos();
        Task<IEnumerable<TicketDTO>> GetClosedTickets();
        Task<TicketDTO> GetTicketById(int id);

        Task<List<TicketDTO>> GetTicketsListByUserId(string userId);

        Task<TicketDTO> GetTicketByClientId(string clientId);
        Task<TicketDTO> GetTicketByUserId(string userId);


        //return tickets por status por usuário
        Task<IEnumerable<TicketDTO>> GetTicketsByUserIdAndStatus(string userId, StatusEnum status);

        //return tickets por status por cliente
        Task<IEnumerable<TicketDTO>> GetTicketsByClientIdAndStatus(string userId, StatusEnum status);

        Task<IEnumerable<TicketDTO>> GetTicketsByStatus(string status);
        Task<IEnumerable<TicketDTO>> GetTicketsByPriority(string priority);
        Task<IEnumerable<TicketDTO>> GetTicketsByNivel(string nivel);


        //retorna qtd de paginas para paginação da pesquisa
        Task<int> GetTotalTicketCountBySearchTerm(string searchTerm);
        //retorna resultado da pesquisa
        //Task<List<TicketDTO>> GetTicketsBySearchTerm(string searchTerm, int pageNumber, int pageSize);
        Task<(List<TicketDTO> Tickets, Dictionary<string, string> ClientAvatars)> GetTicketsBySearchTerm(string searchTerm, int pageNumber, int pageSize);


        Task Add(TicketDTO ticketDto);
        Task Update(TicketDTO ticketDto);
        Task UpdatePartial(TicketDTO ticketDto);
        Task Remove(int id);
    }
}
