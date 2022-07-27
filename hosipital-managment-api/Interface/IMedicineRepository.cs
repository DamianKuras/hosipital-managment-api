using hosipital_managment_api.Models;

namespace hosipital_managment_api.Interface
{
    public interface IMedicineRepository
    {
        public ICollection<Medicine> GetMedicines();
    }
}
