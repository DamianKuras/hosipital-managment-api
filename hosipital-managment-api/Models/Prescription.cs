using hosipital_managment_api.Data;

namespace hosipital_managment_api.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        ApiUser Patient { get; set; }
        ApiUser Doctor { get; set; }
        DateOnly Date { get; set; }
        DateOnly ExpDate { get; set; }
        public List<PrescriptionMedicine> PrescriptionMedicines { get; set; }
    }
}
