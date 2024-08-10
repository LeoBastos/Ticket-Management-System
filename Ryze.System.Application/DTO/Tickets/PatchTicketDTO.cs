using Ryze.System.Domain.Enum;

namespace Ryze.System.Application.DTO.Tickets
{
    public class PatchTicketDTO
    {
        public string? Description { get; set; }
        public string? ClientImage { get; set; }
        public DateTime? OpeningDate { get; set; }
        public string? Resolution { get; set; }
        public string? UserImage { get; set; }
        public StatusEnum? Status { get; set; }
        public NivelEnum? Nivel { get; set; }
        public PriorityEnum? Priority { get; set; }
        public DateTime? ClosingDate { get; set; }
        public string? ClientId { get; set; }
        public string? UserId { get; set; }
        public bool? IsClosed { get; set; }
    }
}
