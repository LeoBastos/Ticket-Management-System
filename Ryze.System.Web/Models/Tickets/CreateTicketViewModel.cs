using Ryze.System.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Ryze.System.Web.helpers;

namespace Ryze.System.Web.Models.Tickets
{
    public class CreateTicketViewModel
    {
        public int Id { get; set; }

        [DisplayName("Descrição do Problema")]
        [Required(ErrorMessage = "Preencha a Descrição")]
        [MinLength(3, ErrorMessage = ("Descrição dever ter 3 ou mais caractéres"))]
        public string Description { get; set; }

        [DisplayName("Imagem do Cliente")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Formato de imagem não suportado. Use jpg, jpeg ou png.")]
        [MaxFileSize((int)(1.2 * 1024 * 1024), ErrorMessage = "A imagem deve ter no máximo 1.2MB.")]
        public IFormFile? ClientImage { get; set; }

        [DisplayName("Dt Abertura")]
        public DateTime OpeningDate { get; set; } = DateTime.Now;

        [DisplayName("Resolução")]
        public string? Resolution { get; set; }

        [DisplayName("Imagem")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Formato de imagem não suportado. Use jpg, jpeg ou png.")]
        [MaxFileSize((int)(1.2 * 1024 * 1024), ErrorMessage = "A imagem deve ter no máximo 1.2MB.")]
        public IFormFile? UserImage { get; set; }

        [DisplayName("Status")]
        public StatusEnum Status { get; set; }

        [DisplayName("Nível")]
        public NivelEnum Nivel { get; set; }

        [DisplayName("Prioridade")]
        public PriorityEnum Priority { get; set; }

        [DisplayName("Dt Fechamento")]
        public DateTime? ClosingDate { get; set; }
        public string ClientId { get; set; }
        public string? UserId { get; set; }

        [DisplayName("Finalizado?")]
        public bool IsClosed { get; set; }
    }
}
