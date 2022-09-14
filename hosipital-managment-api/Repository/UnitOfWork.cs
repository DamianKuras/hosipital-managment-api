using hosipital_managment_api.Data;
using hosipital_managment_api.Interface;
using hosipital_managment_api.Models;

namespace hosipital_managment_api.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private IRepository<Department> _departments;
        private IRepository<Medicine> _medicines;
        private IRepository<Prescription> _prescriptions;
        private IRepository<PrescriptionMedicine> _prescriptionMedicines;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public IRepository<Department> DepartmentRepository => _departments ??= new Repository<Department>(_context,_context.Departments);

        public IRepository<Medicine> MedicineRepository => _medicines ??= new Repository<Medicine>(_context,_context.Medicines);

        public IRepository<Prescription> PrescriptionRepository => _prescriptions ??= new Repository<Prescription>(_context,_context.Prescriptions);

        public IRepository<PrescriptionMedicine> PrescriptionMedicineRepository => _prescriptionMedicines ??= new Repository<PrescriptionMedicine>(_context, _context.PrescriptionMedicines);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }
}
