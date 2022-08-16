using hosipital_managment_api.Models;

namespace hosipital_managment_api.Interface
{
    public interface IPrescriptionRepository
    {
        public Task<IEnumerable<Prescription>> GetPrescriptionsForDoctor(string DoctorId);
        public Task<IEnumerable<Prescription>> GetPrescriptionsForPatient(string PatientId);
        public Task<Prescription> GetPrescription(int id);

        public Task<bool> CreatePrescription(Prescription prescription);
        public Task<bool> UpdatePrescription(Prescription prescription);
        public Task<bool> DeletePrescription(Prescription prescription);
        public Task<bool> PrescriptionExist(int id);
    }
}
