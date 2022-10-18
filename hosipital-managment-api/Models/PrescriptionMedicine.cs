using System.ComponentModel.DataAnnotations;

namespace hosipital_managment_api.Models
{
    public class PrescriptionMedicine
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Prescription Prescription { get; set; }
        public Guid PrescriptionId { get; init; }
        public Medicine Medicine { get; set; }
        public Guid MedicineId { get; init; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "The dosage field must be at least 3 characters long")]
        public string Dosage { get; set; }
    }
}
