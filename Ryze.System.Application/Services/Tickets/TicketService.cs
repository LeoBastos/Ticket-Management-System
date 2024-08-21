using AutoMapper;
using Ryze.System.Application.DTO.Tickets;
using Ryze.System.Domain.Entity.Tickets;
using Ryze.System.Domain.Enum;
using Ryze.System.Domain.Interfaces.Tickets;
using Ryze.System.Domain.Interfaces.UnitOfWork;

namespace Ryze.System.Application.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private ITicketRepository _ticketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketService(IMapper mapper, ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        #region Querys
        public async Task<int> GetTotalTicketCount()
        {
            return await _ticketRepository.GetTotalCountAsync();
        }

        public async Task<List<TicketDTO>> GetPaginatedTickets(int pageNumber, int pageSize)
        {
            var result = await _ticketRepository.GetPaginatedTicketsAsync(pageNumber, pageSize);
            return _mapper.Map<List<TicketDTO>>(result);
        }


        public async Task<int> GetTotalTicketCountByClientId(string clientId)
        {
            return await _ticketRepository.GetTotalTicketCountByClientIdAsync(clientId);
        }

        public async Task<List<TicketDTO>> GetPaginatedTicketsByClientId(string clientId, int pageNumber, int pageSize)
        {
            var result = await _ticketRepository.GetPaginatedTicketsByClientIdAsync(clientId, pageNumber, pageSize);
            return _mapper.Map<List<TicketDTO>>(result);
        }


        public async Task<TicketCountsResult> GetTicketDashboardCountsByAdmin()
        {
            var ticketCounts = await _ticketRepository.GetTicketDashboardCountsByAdminAsync();

            foreach (StatusEnum status in Enum.GetValues(typeof(StatusEnum)))
            {
                if (!ticketCounts.ContainsKey(status))
                {
                    ticketCounts[status] = 0;
                }
            }

            var total = ticketCounts.Values.Sum();

            return new TicketCountsResult
            {
                StatusCounts = ticketCounts,
                Total = total
            };
        }

        public async Task<TicketCountsResult> GetTicketDashboardCountsByClientId(string userId)
        {
            var ticketCounts = await _ticketRepository.GetTicketDashboardCountsByClientIdAsync(userId);

            foreach (StatusEnum status in Enum.GetValues(typeof(StatusEnum)))
            {
                if (!ticketCounts.ContainsKey(status))
                {
                    ticketCounts[status] = 0;
                }
            }

            var total = ticketCounts.Values.Sum();

            return new TicketCountsResult
            {
                StatusCounts = ticketCounts,
                Total = total
            };
        }

        public async Task<TicketCountsResult> GetTicketDashboardCountsByUserId(string userId)
        {
            var ticketCounts = await _ticketRepository.GetTicketDashboardCountsByUserIdAsync(userId);

            foreach (StatusEnum status in Enum.GetValues(typeof(StatusEnum)))
            {
                if (!ticketCounts.ContainsKey(status))
                {
                    ticketCounts[status] = 0;
                }
            }

            var total = ticketCounts.Values.Sum();

            return new TicketCountsResult
            {
                StatusCounts = ticketCounts,
                Total = total
            };
        }

        public async Task<Dictionary<StatusEnum, int>> GetTicketCountsByUserId(string userId)
        {
            var ticketCounts = await _ticketRepository.GetTicketCountsByUserIdAsync(userId);

            foreach (StatusEnum status in Enum.GetValues(typeof(StatusEnum)))
            {
                if (!ticketCounts.ContainsKey(status))
                {
                    ticketCounts[status] = 0;
                }
            }

            return ticketCounts;
        }

        public async Task<IEnumerable<TicketDTO>> GetTickets()
        {
            var result = await _ticketRepository.GetTicketsAsync();
            return _mapper.Map<IEnumerable<TicketDTO>>(result);
        }

        public async Task<IEnumerable<TicketDTO>> GetTicketsInativos()
        {
            var result = await _ticketRepository.GetTicketsDeactiveAsync();
            return _mapper.Map<IEnumerable<TicketDTO>>(result);
        }

        public async Task<IEnumerable<TicketDTO>> GetClosedTickets()
        {
            var result = await _ticketRepository.GetClosedTicketsAsync();
            return _mapper.Map<IEnumerable<TicketDTO>>(result);
        }

        public async Task<TicketDTO> GetTicketById(int id)
        {
            var result = await _ticketRepository.GetTicketByIdAsync(id);

            return _mapper.Map<TicketDTO>(result);
        }

        public async Task<List<TicketDTO>> GetTicketsListByUserId(string userId)
        {
            var tickets = await _ticketRepository.GetTicketsListByUserIdAsync(userId);
            return _mapper.Map<List<TicketDTO>>(tickets);
        }


        public async Task<TicketDTO> GetTicketByClientId(string clientId)
        {
            var result = await _ticketRepository.GetTicketByClientIdAsync(clientId);

            return _mapper.Map<TicketDTO>(result);
        }

        public async Task<TicketDTO> GetTicketByUserId(string userId)
        {
            var result = await _ticketRepository.GetTicketByUserIdAsync(userId);
            return _mapper.Map<TicketDTO>(result);
        }


        //return tickets por status por usuário
        public async Task<IEnumerable<TicketDTO>> GetTicketsByUserIdAndStatus(string userId, StatusEnum status)
        {
            var tickets = await _ticketRepository.GetTicketsByUserIdAndStatusAsync(userId, status);
            return tickets.Select(t => new TicketDTO(t));
        }

        //return tickets por status por cliente
        public async Task<IEnumerable<TicketDTO>> GetTicketsByClientIdAndStatus(string userId, StatusEnum status)
        {
            var tickets = await _ticketRepository.GetTicketsByClientIdAndStatusAsync(userId, status);
            return tickets.Select(t => new TicketDTO(t));
        }

        public async Task<IEnumerable<TicketDTO>> GetTicketsByStatus(string status)
        {
            var result = await _ticketRepository.GetTicketsByStatusAsync(status);
            return _mapper.Map<IEnumerable<TicketDTO>>(result);
        }

        public async Task<IEnumerable<TicketDTO>> GetTicketsByPriority(string priority)
        {
            var result = await _ticketRepository.GetTicketsByPriorityAsync(priority);
            return _mapper.Map<IEnumerable<TicketDTO>>(result);
        }

        public async Task<IEnumerable<TicketDTO>> GetTicketsByNivel(string nivel)
        {
            var result = await _ticketRepository.GetTicketsByNivelAsync(nivel);
            return _mapper.Map<IEnumerable<TicketDTO>>(result);
        }

        //retorna qtd de paginas para paginação da pesquisa
        public async Task<int> GetTotalTicketCountBySearchTerm(string searchTerm)
        {
            return await _ticketRepository.GetTotalTicketCountBySearchTermAsync(searchTerm);
        }
        //retorna resultado da pesquisa
        public async Task<(List<TicketDTO> Tickets, Dictionary<string, string> ClientAvatars)> GetTicketsBySearchTerm(string searchTerm, int pageNumber, int pageSize)
        {
            // Obtém os tickets e as imagens dos clientes
            var (tickets, clientAvatars) = await _ticketRepository.GetTicketsBySearchTermAsync(searchTerm, pageNumber, pageSize);

            // Mapeia os tickets para DTOs
            var ticketDtos = _mapper.Map<List<TicketDTO>>(tickets);

            return (ticketDtos, clientAvatars);
        }


        #endregion

        #region Commands
        public async Task Add(TicketDTO ticketDto)
        {
            try
            {
                var ticket = _mapper.Map<Ticket>(ticketDto);
                await _ticketRepository.CreateAsync(ticket);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();             
                throw new Exception("Failed to add ticket. Description: " + e.Message, e);
            }
        }

        public async Task Update(TicketDTO ticketDto)
        {
            try
            {
                var ticket = _mapper.Map<Ticket>(ticketDto);
                await _ticketRepository.UpdateAsync(ticket);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();                
                throw new Exception("Failed to update ticket. Description: " + e.Message, e);
            }
        }

        public async Task UpdatePartial(TicketDTO ticketDto)
        {
            try
            {
                var ticket = _mapper.Map<Ticket>(ticketDto);
                await _ticketRepository.UpdatePatchAsync(ticket);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();                
                throw new Exception("Failed to partially update ticket. Description: " + e.Message, e);
            }
        }

        public async Task Remove(int id)
        {
            try
            {
                var entity = await _ticketRepository.GetTicketByIdAsync(id);
                entity.Remove();
                await _ticketRepository.UpdateAsync(entity);
                await _unitOfWork.CommitAsync();

                await UpdateTicketCounts();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to remove ticket. Description: " + e.Message, e);
            }
        }

        #endregion

        private async Task UpdateTicketCounts()
        {
            var adminCounts = await GetTicketDashboardCountsByAdmin();
            var clientCounts = await GetTicketDashboardCountsByClientId("clientId");
            var userCounts = await GetTicketDashboardCountsByUserId("userId");
        }
    }
}
