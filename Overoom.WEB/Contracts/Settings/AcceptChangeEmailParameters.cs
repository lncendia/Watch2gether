using System.ComponentModel.DataAnnotations;

namespace Overoom.WEB.Contracts.Settings
{
    public class AcceptChangeEmailParameters
    {
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "Не больше 50 символов")]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [StringLength(200, ErrorMessage = "Не больше 200 символов")]
        public string? Code { get; set; }
    }
}