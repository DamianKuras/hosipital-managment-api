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
            var medicines = _departmentRepository.GetDepartments();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(medicines);
        }
        [HttpGet("{id}")]
        public IActionResult GetDepartment(int id)
        {
            if (!_departmentRepository.DepartmentExist(id))
            {
                return NotFound();
            }
            var medicine = _departmentRepository.GetDepartment(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(medicine);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Department department)
        {
            if (department == null)
                return BadRequest(ModelState);

            if (!_departmentRepository.CreateDepartment(department))
            {
                ModelState.AddModelError("", "Error when adding department to database please try again latter");
                return StatusCode(500, ModelState);
            }
            return Ok("Succesfully created department");
        }
        [HttpPut("{id}")]
        public IActionResult UpdateMedicine(Department department)
        {
            if (department == null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!_departmentRepository.UpdateDepartment(department))
            {
                ModelState.AddModelError("", "Error when updataing medicine please try again latter");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteMedicine(int id)
        {
            if (!_departmentRepository.DepartmentExist(id))
            {
                return NotFound();
            }
            var medicineToDelete = _departmentRepository.GetDepartment(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_departmentRepository.DeleteDepartment(medicineToDelete))
            {
                ModelState.AddModelError("", "Error when deleting department please try again latter");
            }
            return NoContent();
        }
    }
}
