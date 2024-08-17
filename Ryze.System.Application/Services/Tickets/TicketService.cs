using AutoMapper;
using Ryze.System.Application.DTO.Tickets;
using Ryze.System.Domain.Entity.Tickets;
using Ryze.System.Domain.Enum;
using Ryze.System.Domain.Interfaces.Tickets;

namespace Ryze.System.Application.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private ITicketRepository _ticketRepository;

        private readonly IMapper _mapper;

        public TicketService(IMapper mapper, ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
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
        public async Task<List<TicketDTO>> GetTicketsBySearchTerm(string searchTerm, int pageNumber, int pageSize)
        {
            var result = await _ticketRepository.GetTicketsBySearchTermAsync(searchTerm, pageNumber, pageSize);
            return _mapper.Map<List<TicketDTO>>(result);
        }


        #endregion

        #region Commands
        public async Task Add(TicketDTO ticketDto)
        {
            try
            {
                var result = _mapper.Map<Ticket>(ticketDto);
                await _ticketRepository.CreateAsync(result);
            }
            catch (Exception e)
            {
                throw new Exception("Description: " + e);
            }
        }

        public async Task Update(TicketDTO ticketDto)
        {
            try
            {
                var result = _mapper.Map<Ticket>(ticketDto);

                await _ticketRepository.UpdateAsync(result);
            }
            catch (Exception e)
            {
                throw new Exception("Description: " + e);
            }
        }

        public async Task UpdatePartial(TicketDTO ticketDto)
        {
            try
            {
                var result = _mapper.Map<Ticket>(ticketDto);

                await _ticketRepository.UpdatePatchAsync(result);
            }
            catch (Exception e)
            {
                throw new Exception("Description: " + e);
            }
        }

        public async Task Remove(int id)
        {
            var entity = _ticketRepository.GetTicketByIdAsync(id).Result;

            entity.Remove();

            await _ticketRepository.UpdateAsync(entity);
        }

        #endregion
    }
}
