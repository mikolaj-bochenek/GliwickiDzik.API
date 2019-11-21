using System.ComponentModel.DataAnnotations;

namespace GliwickiDzik.DTOs
{
    public class UserForRegisterDTO
    {
        [Required(ErrorMessage="Username cannot be empty!")]
        public string Username { get; set; }
        [Required(ErrorMessage="Password cannot be empty!")]
        [StringLength(12, MinimumLength=6, 
        ErrorMessage="Password has to contain from 6 to 12 characters!")]
        public string Password { get; set; }
    }
}