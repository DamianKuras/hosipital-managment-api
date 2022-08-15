using hosipital_managment_api.Interface;
using hosipital_managment_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace hosipital_managment_api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var departments = await _departmentRepository.GetDepartments();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            if (!await _departmentRepository.DepartmentExist(id))
            {
                return NotFound();
            }
            var department = await _departmentRepository.GetDepartment(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Department department)
        {
            if (department == null)
                return BadRequest(ModelState);

            if (!await _departmentRepository.CreateDepartment(department))
            {
                ModelState.AddModelError("", "Error when adding department to database please try again latter");
                return StatusCode(500, ModelState);
            }
            return Ok("Succesfully created department");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicine(Department department)
        {
            if (department == null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!await _departmentRepository.UpdateDepartment(department))
            {
                ModelState.AddModelError("", "Error when updataing medicine please try again latter");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            if (!await _departmentRepository.DepartmentExist(id))
            {
                return NotFound();
            }
            var medicineToDelete = await _departmentRepository.GetDepartment(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await _departmentRepository.DeleteDepartment(medicineToDelete))
            {
                ModelState.AddModelError("", "Error when deleting department please try again latter");
            }
            return NoContent();
        }
    }
}
