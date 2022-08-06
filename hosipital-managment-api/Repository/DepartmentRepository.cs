using hosipital_managment_api.Data;
using hosipital_managment_api.Interface;
using hosipital_managment_api.Models;

namespace hosipital_managment_api.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;
        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool CreateDepartment(Department department)
        {
            _context.Departments.Add(department);
            return Save();
        }

        public bool DeleteDepartment(Department department)
        {
            _context.Remove(department);
            return Save();
        }

        public bool DepartmentExist(int id)
        {
            return _context.Medicines.Any(p => p.Id == id);
        }

        public Department GetDepartment(int id)
        {
            return _context.Departments.Where(p => p.Id == id).FirstOrDefault();
        }

        public Department GetDepartment(string name)
        {
            return _context.Departments.Where(p => p.Name == name).FirstOrDefault();
        }

        public ICollection<Department> GetDepartments()
        {
            return _context.Departments.ToList();
        }

        public bool UpdateDepartment(Department department)
        {
            _context.Departments.Update(department);
            return Save();
        }


        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
