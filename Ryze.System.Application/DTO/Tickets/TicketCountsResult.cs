using Ryze.System.Domain.Enum;

namespace Ryze.System.Application.DTO.Tickets
{
    public class TicketCountsResult
    {
        public Dictionary<StatusEnum, int> StatusCounts { get; set; }
        public int Total { get; set; }
    }
}
