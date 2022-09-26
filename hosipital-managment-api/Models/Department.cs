using System.ComponentModel.DataAnnotations;

namespace hosipital_managment_api.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The name field must be at least 3 characters long")]
        public string Name { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "The description field must be at least 3 characters long")]
        public string Descritpion { get; set; }
    }
}
