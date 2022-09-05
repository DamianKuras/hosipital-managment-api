namespace hosipital_managment_api.Dto
{
    public class PrescriptionMedicineDTO
    {
        public string Dosage { get; set; }
        public int MedicineId { get; set; }
        public int Quantity { get; set; }

    }

    public class PrescriptionMedicineDisplayDTO
    {
        public string Name { get; set; }
        public string Strength { get; set; }
        public string Description { get; set; }
        public string RouteOfAdministration { get; set; }
        public string Dosage { get; set; }
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
    }
}
