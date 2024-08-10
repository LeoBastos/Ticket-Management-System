using Ryze.System.Domain.Entity.Identity;
using Ryze.System.Domain.Entity.Tickets;
using Ryze.System.Domain.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ryze.System.Application.DTO.Tickets
{
    public class TicketDTO
    {
        public TicketDTO()
        {

        }

        public TicketDTO(int id, string description, string? clientImage, DateTime openingDate, string? resolution, string? userImage,
            StatusEnum status, NivelEnum nivel, PriorityEnum priority, DateTime? closingDate, string clientId,
            string? userId, string clientName, string userName, ApplicationUser client, ApplicationUser user)
        {
            Id = id;
            Description = description;
            ClientImage = clientImage;
            OpeningDate = openingDate;
            Resolution = resolution;
            UserImage = userImage;
            Status = status;
            Nivel = nivel;
            Priority = priority;
            ClosingDate = closingDate;
            ClientId = clientId;
            UserId = userId;
            ClientName = clientName;
            UserName = userName;
            Client = client;
            User = user;
        }

        public TicketDTO(Ticket ticket)
        {
            Id = ticket.Id;
            Description = ticket.Description;
            ClientImage = ticket.ClientImage;
            OpeningDate = ticket.OpeningDate;
            Resolution = ticket.Resolution;
            UserImage = ticket.UserImage;
            Status = ticket.Status;
            Nivel = ticket.Nivel;
            Priority = ticket.Priority;
            ClosingDate = ticket.ClosingDate;
            ClientId = ticket.ClientId;
            UserId = ticket.UserId;
            Client = ticket.Client;
            User = ticket.User;
        }

        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe a Descrição do ticket")]
        [MinLength(3)]
        [DisplayName("Descrição")]
        public string Description { get; set; }

        [DisplayName("Imagem")]
        public string? ClientImage { get; set; }

        public DateTime OpeningDate { get; set; } = DateTime.Now.Date;

        [MinLength(3)]
        [DisplayName("Resolução")]
        public string? Resolution { get; set; }

        [DisplayName("Imagem")]
        public string? UserImage { get; set; }

        [DisplayName("Status")]
        public StatusEnum Status { get; set; } = StatusEnum.EmAberto;

        [DisplayName("Nível")]
        public NivelEnum Nivel { get; set; }

        [DisplayName("Prioridade")]
        public PriorityEnum Priority { get; set; }

        [DisplayName("Data Fechamento")]
        public DateTime? ClosingDate { get; set; }

        [DisplayName("Cliente Id")]
        public string ClientId { get; set; }

        [DisplayName("Usuario Id")]
        public string? UserId { get; set; }

        [DisplayName("Cliente")]
        public string ClientName { get; set; }

        [DisplayName("Funcionário")]
        public string UserName { get; set; }

        public virtual ApplicationUser Client { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
