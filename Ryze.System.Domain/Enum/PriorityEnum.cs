using System.ComponentModel.DataAnnotations;

namespace Ryze.System.Domain.Enum
{
    public enum PriorityEnum
    {
        [Display(Name = "Baixa")]
        Baixa,
        [Display(Name = "Média")]
        Media,
        [Display(Name = "Alta")]
        Alta
    }
}
