using hosipital_managment_api.Models;

namespace hosipital_managment_api.Interface
{
    public interface IMedicineRepository
    {
        public ICollection<Medicine> GetMedicines();
        public Medicine GetMedicine(int id);
        public Medicine GetMedicine(string name);
        public bool CreateMedicine(Medicine medicine);
        public bool UpdateMedicine(Medicine medicine);
        public bool DeleteMedicine(Medicine medicine);
        public bool MedicineExist(int id);
    }
}
