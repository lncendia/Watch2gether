using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.Settings
{
    public class ChangePasswordParameters
    {
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Не больше 50 символов")]
        [Display(Name = "Старый пароль")]
        public string? OldPassword { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Не больше 50 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Не больше 50 символов")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string? PasswordConfirm { get; set; }
    }
}