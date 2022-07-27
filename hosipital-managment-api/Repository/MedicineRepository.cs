using hosipital_managment_api.Data;
using hosipital_managment_api.Models;
using hosipital_managment_api.Interface;

namespace hosipital_managment_api.Repository
{
    public class MedicineRepository:IMedicineRepository
    {
        private readonly AppDbContext _context;
        public MedicineRepository(AppDbContext context)
        {
            _context = context;
        }
        public ICollection<Medicine> GetMedicines()
        {
            return _context.Medicines.ToList();
        }

    }
}
