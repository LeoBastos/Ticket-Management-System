using Ryze.System.Domain.Entity.Identity;
using Ryze.System.Domain.Enum;
using System.ComponentModel;

namespace Ryze.System.Web.Models.Tickets
{
    public class TicketViewModel
    {
        public int Id { get; set; }

        [DisplayName("Descrição")]
        public string Description { get; set; }

        [DisplayName("Avatar")]
        public string? ClientImage { get; set; }

        [DisplayName("Dt. Abertura")]
        public DateTime OpeningDate { get; set; } = DateTime.Now.Date;

        [DisplayName("Resolução")]
        public string? Resolution { get; set; }

        [DisplayName("Avatar")]
        public string? UserImage { get; set; }
        public StatusEnum Status { get; set; }
        public NivelEnum Nivel { get; set; }

        [DisplayName("Prioridade")]
        public PriorityEnum Priority { get; set; }

        [DisplayName("Dt. Fechamento")]
        public DateTime? ClosingDate { get; set; }
        public string ClientId { get; set; }
        public string? UserId { get; set; }
        public bool IsClosed { get; set; }

        [DisplayName("Cliente")]
        public string ClientName { get; set; }

        [DisplayName("Funcionário")]
        public string? UserName { get; set; }

        public virtual ApplicationUser Client { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public string SortOrder { get; set; }
    }
}
