using hosipital_managment_api.Models;

namespace hosipital_managment_api.Interface
{
    public interface IMedicineRepository
    {
        public Task<IEnumerable<Medicine>> GetMedicines();
        public Task<Medicine> GetMedicine(int id);
        public Task<Medicine> GetMedicine(string name);
        public Task<bool> CreateMedicine(Medicine medicine);
        public Task<bool> UpdateMedicine(Medicine medicine);
        public Task<bool> DeleteMedicine(Medicine medicine);
        public Task<bool> MedicineExist(int id);
    }
}
