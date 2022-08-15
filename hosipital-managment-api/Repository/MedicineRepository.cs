using hosipital_managment_api.Data;
using hosipital_managment_api.Models;
using hosipital_managment_api.Interface;
using Microsoft.EntityFrameworkCore;

namespace hosipital_managment_api.Repository
{
    public class MedicineRepository:IMedicineRepository
    {
        private readonly AppDbContext _context;
        public MedicineRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Medicine>> GetMedicines()
        {
            return await _context.Medicines.ToListAsync();
        }

        public async Task<Medicine> GetMedicine(int id)
        {
            return await _context.Medicines.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Medicine> GetMedicine(string name)
        {
            return await _context.Medicines.Where(p => p.Name == name).FirstOrDefaultAsync();
        }

        public async Task<bool> CreateMedicine(Medicine medicine)
        {
            _context.Medicines.Add(medicine);
            return await Save();
        }

        public async Task<bool> UpdateMedicine(Medicine medicine)
        {
            _context.Update(medicine);
            return await Save();
        }

        public async Task<bool> DeleteMedicine(Medicine medicine)
        {
            _context.Remove(medicine);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> MedicineExist(int id)
        {
            return await _context.Medicines.AnyAsync(p=>p.Id == id);
        }
    }
}
