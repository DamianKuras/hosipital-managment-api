using hosipital_managment_api.ActionFilters;
using hosipital_managment_api.Interface;
using hosipital_managment_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace hosipital_managment_api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Get([FromQuery] PagingParameters pagingParameters)
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAll(pagingParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(departments.PagingMetadata));
            return Ok(departments);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
            return Ok(department);
        }

        [DepartmentValidationFilter]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] Department department)
        {
            _unitOfWork.DepartmentRepository.Add(department);
            if (!await _unitOfWork.Save())
            {
                ModelState.AddModelError("", "Error when adding department to database please try again latter");
                return StatusCode(500, ModelState);
            }
            return CreatedAtAction(nameof(Get),new {id=department.Id},department);
        }

        [DepartmentValidationFilter]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id,[FromBody] Department department)
        {
            var departmentToUpdate = _unitOfWork.DepartmentRepository.GetById(id);
            if(departmentToUpdate == null || id != department.Id)
            {
                return BadRequest(ModelState);
            }
            _unitOfWork.DepartmentRepository.Update(department);
            if (!await _unitOfWork.Save())
            {
                ModelState.AddModelError("error: ", "Error when updataing department please try again latter");
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
