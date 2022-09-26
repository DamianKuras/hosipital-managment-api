using System.ComponentModel.DataAnnotations;

namespace hosipital_managment_api.Models
{
    public class PrescriptionMedicine
    {
        public int Id { get; set; }

        public Prescription Prescription { get; set; }
        public int PrescriptionId { get; set; }
        public Medicine Medicine { get; set; }
        public int MedicineId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "The dosage field must be at least 3 characters long")]
        public string Dosage { get; set; }
    }
}
