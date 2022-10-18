using System.ComponentModel.DataAnnotations;

namespace hosipital_managment_api.Models
{
    public class Medicine
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MinLength(3, ErrorMessage = "The name field must be at least 3 characters long")]
        public string Name { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The strength field must be at least 3 characters long")]
        public string Strength { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "The description field must be at least 6 characters long")]
        public string Description { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "The route of admininitstartion field must be at least 3 characters long")]
        public string RouteOfAdministration { get; set; }
    }
}
