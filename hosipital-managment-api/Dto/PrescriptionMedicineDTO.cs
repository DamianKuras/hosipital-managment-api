namespace hosipital_managment_api.Dto
{
    public class PrescriptionMedicineDTO
    {
        public string Dosage { get; set; }
        public Guid MedicineId { get; set; }
        public int Quantity { get; set; }

    }

    public class PrescriptionMedicineDisplayDTO
    {
        public string MedicineName { get; set; }
        public string MedicineStrength { get; set; }
        public string MedicineDescription { get; set; }
        public string MedicineRouteOfAdministration { get; set; }
        public string Dosage { get; set; }
        public int Quantity { get; set; }
    }
}
