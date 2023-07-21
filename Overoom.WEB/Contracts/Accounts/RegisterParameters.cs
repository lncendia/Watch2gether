using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.Accounts;

public class RegisterParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Введите имя пользователя")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Имя пользователя должно содержать от 3 до 20 символов")]
    [RegularExpression("^[a-zA-Zа-яА-Я0-9_ ]+$",
        ErrorMessage =
            "Имя может содержать только латинские или кириллические буквы, цифры, пробелы и символы подчеркивания")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [DataType(DataType.EmailAddress, ErrorMessage="Некорректный формат почты")]
    [Display(Name = "Введите электронный адрес")]
    [StringLength(50, ErrorMessage = "Не больше 50 символов")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [DataType(DataType.Password)]
    [Display(Name = "Введите пароль")]
    [StringLength(30, MinimumLength = 8, ErrorMessage = "От 8 до 30 символов")]
    [RegularExpression("^(?=.*?[A-Za-z])^(?=.*?[0-9])^(?=.*?[^a-zA-Z0-9])[a-zA-Z0-9_\\/\\*.#]+$",
        ErrorMessage = "Пароль должен содержать буквы, цифры и специальные символы и не может иметь разрывов")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(30, MinimumLength = 8, ErrorMessage = "От 8 до 30 символов")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль")]
    public string? PasswordConfirm { get; set; }
}