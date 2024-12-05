using System.ComponentModel.DataAnnotations;

namespace Bank.DTO
{
    public class CreateUserDTO
    {
        [Required]
        [MaxLength(50,ErrorMessage = "Max Length is 50")]
        [MinLength(3, ErrorMessage = "Min Length is 3")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Max Length is 50")]
        [MinLength(3, ErrorMessage = "Min Length is 3")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Max Username Length is 50")]
        [MinLength(3, ErrorMessage = "Min Username Length is 3")]
        public string Username { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Max Password Length is 50")]
        [MinLength(12, ErrorMessage = "Min Password Length is 12")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Max Password Length is 50")]
        [MinLength(12, ErrorMessage = "Min Password Length is 12")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
