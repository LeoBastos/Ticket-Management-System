using System.ComponentModel.DataAnnotations;

namespace Ryze.System.Domain.Enum
{
    public enum StatusEnum
    {
        [Display(Name = "Em Aberto")]
        EmAberto = 0,
        [Display(Name = "Em Andamento")]
        EmAndamento = 1,
        [Display(Name = "Fechado")]
        Fechado = 2
    }
}
