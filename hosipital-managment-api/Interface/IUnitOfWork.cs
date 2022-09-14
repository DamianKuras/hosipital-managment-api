using hosipital_managment_api.Models;

namespace hosipital_managment_api.Interface
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<Department> DepartmentRepository { get; }
        IRepository<Medicine> MedicineRepository { get; }
        IRepository<Prescription> PrescriptionRepository { get; }
        IRepository<PrescriptionMedicine> PrescriptionMedicineRepository { get; }
        Task<bool> Save();
    }
}
