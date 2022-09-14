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
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Department))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAll();
            if(departments == null)
            {
                return NoContent();
            }
            return Ok(departments);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Department))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetById(id);
            if(department == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(department);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] Department department)
        {
            if (department == null)
                return BadRequest(ModelState);
            await _unitOfWork.DepartmentRepository.Add(department);
            if (!await _unitOfWork.Save())
            {
                ModelState.AddModelError("", "Error when adding department to database please try again latter");
                return StatusCode(500, ModelState);
            }
            return CreatedAtAction(nameof(Get),new {id=department.Id},department);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id,[FromBody] Department department)
        {
            if (department == null || id != department.Id)
                return BadRequest(ModelState);
            var departmentToUpdate = _unitOfWork.DepartmentRepository.GetById(id);
            if(departmentToUpdate == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _unitOfWork.DepartmentRepository.Update(department);

            if (!await _unitOfWork.Save())
            {
                ModelState.AddModelError("", "Error when updataing department please try again latter");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var departmentToDelete = await _unitOfWork.DepartmentRepository.GetById(id);
            if (departmentToDelete == null)
            {
                return NotFound();
            }
            _unitOfWork.DepartmentRepository.Delete(departmentToDelete);
            if (!await _unitOfWork.Save())
            {
                ModelState.AddModelError("", "Error when deleting department please try again latter");
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
