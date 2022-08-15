using hosipital_managment_api.Models;

namespace hosipital_managment_api.Interface
{
    public interface IDepartmentRepository
    {
        public Task<IEnumerable<Department>> GetDepartments();
        public Task<Department> GetDepartment(int id);
        public Task<Department> GetDepartment(string name);
        public Task<bool> CreateDepartment(Department department);
        public Task<bool> UpdateDepartment(Department department);
        public Task<bool> DeleteDepartment(Department department);
        public Task<bool> DepartmentExist(int id);
    }
}
