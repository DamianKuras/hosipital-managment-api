using hosipital_managment_api.ActionFilters;
using hosipital_managment_api.Data;
using hosipital_managment_api.Interface;
using hosipital_managment_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace hosipital_managment_api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "Doctor,Nurse,Pharmacist")]
    [Produces("application/json")]
    public class MedicineController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public MedicineController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Medicine))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get([FromQuery] PagingParameters pagingParameters)
        {
            var medicines = await _unitOfWork.MedicineRepository.GetAll(pagingParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(medicines.PagingMetadata));
            return Ok(medicines);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Medicine))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get(Guid id)
        {
            var medicine = await _unitOfWork.MedicineRepository.GetById(id);
            if (medicine == null)
            {
                return NotFound();
            }
            return Ok(medicine);
        }

        [MedicineValidationFilter]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(Medicine medicine)
        {
            _unitOfWork.MedicineRepository.Add(medicine);
            if (!await _unitOfWork.Save())
            {
                ModelState.AddModelError("", "Error when adding medicine to database please try again latter");
                return StatusCode(500, ModelState);
            }
            return CreatedAtAction(nameof(Get), new { id = medicine.Id }, medicine);
        }

        [MedicineValidationFilter]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Medicine medicine)
        {
            _unitOfWork.MedicineRepository.Update(medicine);
            if (!await _unitOfWork.Save())
            {
                ModelState.AddModelError("", "Error when updataing medicine please try again latter");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMedicine(Guid id)
        {
            var medicineToDelete = await _unitOfWork.MedicineRepository.GetById(id);
            if (medicineToDelete == null)
            {
                return NotFound();
            }
            _unitOfWork.MedicineRepository.Delete(medicineToDelete);
            if (!await _unitOfWork.Save())
            {
                ModelState.AddModelError("", "Error when deleting medicine please try again latter");
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
