using hosipital_managment_api.Data;
using hosipital_managment_api.Interface;
using hosipital_managment_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hosipital_managment_api.Controllers
{
    [Authorize(Roles = "Doctor,Nurse,Pharmacist")]
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineRepository _medicineRepository;
        public MedicineController(IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Medicine))]
        public async Task<IActionResult> Get()
        {
            var medicines = await _medicineRepository.GetMedicines();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(medicines);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Medicine))]
        public async Task<IActionResult> GetMedicine(int id)
        {

            if (! await _medicineRepository.MedicineExist(id))
            {
                return NotFound();
            }
            var medicine= await _medicineRepository.GetMedicine(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(medicine);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkObjectResult))]
        public async Task<IActionResult> Post(Medicine medicine)
        {
            if (medicine == null)
                return BadRequest(ModelState);

            if (!await _medicineRepository.CreateMedicine(medicine))
            {
                ModelState.AddModelError("", "Error when adding medicine to database please try again latter");
                return StatusCode(500, ModelState);
            }
            return Ok("Succesfully created medicine");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateMedicine(Medicine medicine)
        {
            if (medicine == null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!await _medicineRepository.UpdateMedicine(medicine))
            {
                ModelState.AddModelError("", "Error when updataing medicine please try again latter");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            if (!await _medicineRepository.MedicineExist(id))
            {
                return NotFound();
            }
            var medicineToDelete = await _medicineRepository.GetMedicine(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await _medicineRepository.DeleteMedicine(medicineToDelete))
            {
                ModelState.AddModelError("", "Error when deleting medicine please try again latter");
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
