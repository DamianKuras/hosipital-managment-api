using hosipital_managment_api.Data;
using hosipital_managment_api.Interface;
using hosipital_managment_api.Models;
using Microsoft.EntityFrameworkCore;

namespace hosipital_managment_api.Repository
{
    public class PrescriptionMedicineRepository : IPrescriptionMedicineRepository
    {
        private readonly AppDbContext _context;
        public PrescriptionMedicineRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreatePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        {
            _context.PrescriptionMedicines.Add(prescriptionMedicine);
            return await Save();
        }

        public async Task<bool> DeletePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        {
            _context.PrescriptionMedicines.Remove(prescriptionMedicine);
            return await Save();
        }

        public async Task<PrescriptionMedicine> GetPrescriptionMedicine(int id)
        {
            return await _context.PrescriptionMedicines.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> PrescriptionMedicineExist(int id)
        {
            return await _context.PrescriptionMedicines.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> UpdatePrescriptionMedicine(PrescriptionMedicine prescriptionMedicine)
        {
            _context.Update(prescriptionMedicine);
            return await Save();
        }
        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<IEnumerable<PrescriptionMedicine>> GetPrescriptionMedicineForPrescription(int id)
        {
            return await _context.PrescriptionMedicines.Where(p => p.PrescriptionId == id).ToListAsync();
        }
    }
}
