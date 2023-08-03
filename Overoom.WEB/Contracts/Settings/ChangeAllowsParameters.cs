using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.Settings
{
    public class ChangeAllowsParameters
    {
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Звуковой сигнал")]
        public bool Beep { get; set; } = true;

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Скример")]
        public bool Scream { get; set; } = true;
        
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Изменение имени")]
        public bool Change { get; set; } = true;
    }
}