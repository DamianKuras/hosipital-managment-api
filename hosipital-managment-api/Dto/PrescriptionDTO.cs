namespace hosipital_managment_api.Dto
{
    public class PrescriptionDTO
    {
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public List<PrescriptionMedicineDTO> PrescriptionMedicinesDTO { get; set; }
    }

    public class PrescriptionDisplayDTO
    {
        public DateTimeOffset Created_at { get; set; }
        public DateOnly ExpDate { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public DateOnly PatientBirthDay { get; set; }
        public string PatientCity { get; set; }
        public string PatientStreet { get; set; }
        public string PatientPostcode { get; set; }
        public string PatientPhoneNumber { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }
        public string DoctorPhoneNumber { get; set; }

        public IEnumerable<PrescriptionMedicineDisplayDTO> PrescriptionMedicinesDTO { get; set; }

    }
    public class PrescriptionsListDisplayDTO
    {
        public Guid Id { get; set; }
        public DateTimeOffset Created_at { get; set; }
        public DateOnly ExpDate { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }

    }
}
