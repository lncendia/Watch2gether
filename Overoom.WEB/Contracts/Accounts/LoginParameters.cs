using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.Accounts;

public class LoginParameters
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(50, ErrorMessage = "Не больше 50 символов")]
    [DataType(DataType.EmailAddress, ErrorMessage="Некорректный формат почты")]
    [Display(Name = "Почта")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(30, MinimumLength = 8, ErrorMessage = "От 8 до 30 символов")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string? Password { get; set; }

    [Display(Name = "Запомнить меня")] public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; } = "/";
}