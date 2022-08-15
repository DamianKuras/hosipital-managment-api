using hosipital_managment_api.Data;
using hosipital_managment_api.Interface;
using hosipital_managment_api.Models;
using Microsoft.EntityFrameworkCore;

namespace hosipital_managment_api.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;
        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateDepartment(Department department)
        {
            _context.Departments.Add(department);
            return await Save();
        }

        public async Task<bool> DeleteDepartment(Department department)
        {
            _context.Remove(department);
            return await Save();
        }

        public async Task<bool> DepartmentExist(int id)
        {
            return await _context.Departments.AnyAsync(p => p.Id == id);
        }

        public async Task<Department> GetDepartment(int id)
        {
            return await _context.Departments.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Department> GetDepartment(string name)
        {
            return await _context.Departments.Where(p => p.Name == name).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<bool> UpdateDepartment(Department department)
        {
            _context.Departments.Update(department);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
