using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.Accounts;

public class ResetPasswordParameters
{
    [StringLength(50, ErrorMessage = "Не больше 50 символов")]
    [Required(ErrorMessage = "Поле не должно быть пустым")]
    [DataType(DataType.EmailAddress, ErrorMessage="Некорректный формат почты")]
    [Display(Name = "Введите электронный адрес, к которому привязан аккаунт.")]
    public string? Email { get; set; }
}