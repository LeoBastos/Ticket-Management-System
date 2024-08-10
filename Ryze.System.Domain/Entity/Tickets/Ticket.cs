using Ryze.System.Domain.Entity.Identity;
using Ryze.System.Domain.Enum;
using Ryze.System.Domain.Validation;

namespace Ryze.System.Domain.Entity.Tickets
{
    public class Ticket : BaseEntity
    {
        private Ticket()
        {

        }

        public Ticket(string description, string clientImage, DateTime openingDate,
                    string resolution, string userImage, StatusEnum status, NivelEnum nivel, PriorityEnum priority, DateTime? closingDate,
                    string clientId, string userId)
        {
            ValidateDomain(description, clientImage, openingDate, resolution, userImage, status, nivel, priority, closingDate, clientId, userId);
        }

        public string Description { get; private set; }
        public string? ClientImage { get; private set; }
        public DateTime OpeningDate { get; private set; } = DateTime.Now;
        public string? Resolution { get; private set; }
        public string? UserImage { get; private set; }
        public StatusEnum Status { get; private set; }
        public NivelEnum Nivel { get; private set; }
        public PriorityEnum Priority { get; private set; }
        public DateTime? ClosingDate { get; private set; }
        public string ClientId { get; private set; }
        public string? UserId { get; private set; }

        public virtual ApplicationUser Client { get; set; }
        public virtual ApplicationUser User { get; set; }




        public void Update(string description, string clientImage, DateTime openingDate,
                    string resolution, string userImage, StatusEnum status, NivelEnum nivel, PriorityEnum priority, DateTime? closingDate,
                    string clientId, string userId)
        {
            ValidateDomain(description, clientImage, openingDate, resolution, userImage, status, nivel, priority, closingDate, clientId, userId);
        }

        public void PatchUpdate(string userId)
        {
            UserId = userId;
        }

        public void Remove()
        {
            IsActive = false;
        }

        private void ValidateDomain(string description, string clientImage, DateTime openingDate,
                    string resolution, string userImage, StatusEnum status, NivelEnum nivel, PriorityEnum priority, DateTime? closingDate,
                    string clientId, string userId)
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(description), "Campo DESCRIÇÃO deve ser Preenchido.");
            DomainExceptionValidation.When(description.Length < 3, "Campo DESCRIÇÃO deve conter mais do que 3 caracteres.");

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
        }
    }
}
