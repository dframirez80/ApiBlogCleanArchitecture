using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = ErrorMessage.Email)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(ErrorMessage.MaxLengthPassword,
              MinimumLength = ErrorMessage.MinLengthPassword,
              ErrorMessage = "Debe ingresar {0} y {1} caracteres")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(ErrorMessage.MaxLengthPassword,
              MinimumLength = ErrorMessage.MinLengthPassword,
              ErrorMessage = "Debe ingresar {0} y {1} caracteres")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
