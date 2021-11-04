using Domain.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Dtos
{
    public class UserDto
    {
        [Required(ErrorMessage = ErrorMessage.Names)]
        public string Names { get; set; }
        
        [Required(ErrorMessage = ErrorMessage.Surnames)]
        public string Surnames { get; set; }
        
        [Required(ErrorMessage = ErrorMessage.Email)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(ErrorMessage.MaxLengthPassword, 
                      MinimumLength = ErrorMessage.MinLengthPassword, 
                      ErrorMessage = "Debe ingresar {0} y {1} caracteres")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
    }
}
