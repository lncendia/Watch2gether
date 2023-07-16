using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.Accounts;

public class RegisterParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [Display(Name = "Введите имя пользователя")]
    [RegularExpression("^[a-zA-Zа-яА-Я0-9_ ]{3,20}$", ErrorMessage = "Имя пользователя должно содержать от 3 до 20 символов")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Введите электронный адрес")]
    [StringLength(50, ErrorMessage = "Не больше 50 символов")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [DataType(DataType.Password)]
    [Display(Name = "Введите пароль")]
    [StringLength(50, ErrorMessage = "Не больше 50 символов")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(50, ErrorMessage = "Не больше 50 символов")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль")]
    public string? PasswordConfirm { get; set; }
}