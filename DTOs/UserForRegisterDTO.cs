using System;
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
        [Required(ErrorMessage="Email cannot be empty!")]
        public string Email { get; set; }
        [Required(ErrorMessage="Select your gender!")]
        public string Gender { get; set; }
        [Required(ErrorMessage="Select your date of birth!")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage="Type your biceps size!")]
        public float BicepsSize { get; set; }
        public DateTime DateOfCreated { get; set; }
        public DateTime LastActive { get; set; }

        public UserForRegisterDTO()
        {
            DateOfCreated = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}