using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.Settings
{
    public class ChangePasswordParameters
    {
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Не больше 30 символов")]
        [Display(Name = "Старый пароль")]
        public string? OldPassword { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Не больше 30 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        [RegularExpression("^(?=.*?[A-Za-z])^(?=.*?[0-9])^(?=.*?[^a-zA-Z0-9])[a-zA-Z0-9_\\/\\*.#]+$", ErrorMessage="Пароль должен содержать буквы, цифры и специальные символы и не может иметь разрывов")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "От 8 до 30 символов")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string? PasswordConfirm { get; set; }
    }
}