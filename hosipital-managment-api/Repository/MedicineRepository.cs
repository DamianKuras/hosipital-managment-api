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
        public Medicine GetMedicine(int id)
        {
            return _context.Medicines.Where(p => p.Id == id).FirstOrDefault();
        }
        public Medicine GetMedicine(string name)
        {
            return _context.Medicines.Where(p=>p.Name == name).FirstOrDefault();
        }
        public bool CreateMedicine(Medicine medicine)
        {
            _context.Medicines.Add(medicine);
            return Save();

        }


        public bool UpdateMedicine(Medicine medicine)
        {
            _context.Update(medicine);
            return Save();
        }

        public bool DeleteMedicine(Medicine medicine)
        {
            _context.Remove(medicine);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool MedicineExist(int id)
        {
            return _context.Medicines.Any(p=>p.Id == id);
        }
    }
}
