using hosipital_managment_api.Data;
using hosipital_managment_api.Interface;
using hosipital_managment_api.Models;
using Microsoft.EntityFrameworkCore;

namespace hosipital_managment_api.Repository
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly AppDbContext _context;
        public PrescriptionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreatePrescription(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            return await Save();
        }

        public async Task<bool> DeletePrescription(Prescription prescription)
        {
            _context.Remove(prescription);
            return await Save();
        }

        public async Task<Prescription> GetPrescription(int id)
        {
            return await _context.Prescriptions.Where(p => p.Id == id).Include(p => p.Doctor).Include(p => p.Patient).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Prescription>> GetPrescriptionsForDoctor(string DoctorId)
        {
            return await _context.Prescriptions.Where(p => p.Doctor.Id == DoctorId).Include(p => p.Doctor).Include(p => p.Patient).ToListAsync();
        }

        public async Task<IEnumerable<Prescription>> GetPrescriptionsForPatient(string PatientId)
        {
            return await _context.Prescriptions.Where(p => p.Patient.Id == PatientId).Include(p => p.Doctor).Include(p => p.Patient).ToListAsync();
        }

        public async Task<bool> PrescriptionExist(int id)
        {
            return await _context.Prescriptions.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdatePrescription(Prescription prescription)
        {
            _context.Update(prescription);
            return await Save();
        }
    }
}
