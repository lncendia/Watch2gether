using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Models.Accounts;

public class LoginViewModel
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(50, ErrorMessage = "Не больше 50 символов")]
    [Display(Name = "Почта")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(50, ErrorMessage = "Не больше 50 символов")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = null!;

    [Display(Name = "Запомнить меня")] public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; } = "/";
}