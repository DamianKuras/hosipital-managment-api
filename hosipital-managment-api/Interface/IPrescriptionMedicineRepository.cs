using hosipital_managment_api.Models;

namespace hosipital_managment_api.Interface
{
    public interface IPrescriptionMedicineRepository
    {
        public Task<PrescriptionMedicine> GetPrescriptionMedicine(int id);
        public Task<IEnumerable<PrescriptionMedicine>> GetPrescriptionMedicineForPrescription(int id);

        public Task<bool> CreatePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine);
        public Task<bool> UpdatePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine);
        public Task<bool> DeletePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine);
        public Task<bool> PrescriptionMedicineExist(int id);
    }
}
