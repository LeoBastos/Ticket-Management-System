using Ryze.System.Domain.Enum;
using Ryze.System.Web.helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Ryze.System.Web.Models.Tickets
{
    public class EditTicketViewModel
    {
        public int Id { get; set; }
        [DisplayName("Dt. Abertura")]
        public DateTime OpeningDate { get; set; }

        [Required]
        [DisplayName("Descrição do Problema")]
        public string Description { get; set; }

        [DisplayName("Imagem")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Formato de imagem não suportado. Use jpg, jpeg ou png.")]
        [MaxFileSize((int)(1.2 * 1024 * 1024), ErrorMessage = "A imagem deve ter no máximo 1.2MB.")]
        public IFormFile? ClientImage { get; set; }
        public string? ClientImageUrl { get; set; }

        [DisplayName("Imagem")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Formato de imagem não suportado. Use jpg, jpeg ou png.")]
        [MaxFileSize((int)(1.2 * 1024 * 1024), ErrorMessage = "A imagem deve ter no máximo 1.2MB.")]
        public IFormFile? UserImage { get; set; }
        public string? UserImageUrl { get; set; }

        [DisplayName("Status")]
        public StatusEnum Status { get; set; }

        [DisplayName("Nivel")]
        public NivelEnum Nivel { get; set; }

        [DisplayName("Prioridade")]
        public PriorityEnum Priority { get; set; }

        [DisplayName("Resolução")]
        public string? Resolution { get; set; }

        [DisplayName("Dt. Fechamento")]
        public DateTime? ClosingDate { get; set; }

        [DisplayName("Finalizado?")]
        public bool IsClosed { get; set; }

        public string ClientId { get; set; }
        public string? UserId { get; set; }
    }
}
