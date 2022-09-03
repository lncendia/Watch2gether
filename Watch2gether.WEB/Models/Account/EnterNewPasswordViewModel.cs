using System.ComponentModel.DataAnnotations;

namespace Watch2gether.WEB.Models.Account;

public class EnterNewPasswordViewModel
{
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [DataType(DataType.Password)]
    [StringLength(50, ErrorMessage = "Не больше 50 символов")]
    [Display(Name = "Новый пароль")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [StringLength(50, ErrorMessage = "Не больше 50 символов")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль")]
    public string PasswordConfirm { get; set; } = null!;

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Поле не должно быть пустым")]
    public string Code { get; set; } = null!;
}