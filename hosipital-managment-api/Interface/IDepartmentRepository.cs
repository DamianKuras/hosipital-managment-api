using hosipital_managment_api.Models;

namespace hosipital_managment_api.Interface
{
    public interface IDepartmentRepository
    {
        public ICollection<Department> GetDepartments();
        public Department GetDepartment(int id);
        public Department GetDepartment(string name);
        public bool CreateDepartment(Department department);
        public bool UpdateDepartment(Department department);
        public bool DeleteDepartment(Department department);
        public bool DepartmentExist(int id);
    }
}
