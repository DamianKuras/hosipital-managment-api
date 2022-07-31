using System.ComponentModel.DataAnnotations;

namespace hosipital_managment_api.Dto
{
    public class UserDto
    {
        [Required]
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Your password needs to be between {1} and {2} characters",MinimumLength =5)]
        public string Password { get; set; }
    }
}
