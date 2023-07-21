using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.Accounts;

public class NewPasswordParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [DataType(DataType.Password)]
    [StringLength(30, MinimumLength = 8, ErrorMessage = "От 8 до 30 символов")]
    [RegularExpression("^(?=.*?[A-Za-z])^(?=.*?[0-9])^(?=.*?[^a-zA-Z0-9])[a-zA-Z0-9_\\/\\*.#]+$", ErrorMessage="Пароль должен содержать буквы, цифры и специальные символы и не может иметь разрывов")]
    [Display(Name = "Новый пароль")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(30, MinimumLength = 8, ErrorMessage = "От 8 до 30 символов")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль")]
    public string? PasswordConfirm { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [DataType(DataType.EmailAddress, ErrorMessage="Некорректный формат почты")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    public string? Code { get; set; }
}