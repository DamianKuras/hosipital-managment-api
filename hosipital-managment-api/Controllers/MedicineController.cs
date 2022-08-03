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
        public async Task<IActionResult> Get()
        {
            var medicines = _medicineRepository.GetMedicines();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(medicines);
        }
        [HttpGet("{id}")]
        public IActionResult GetMedicine(int id)
        {
            if (!_medicineRepository.MedicineExist(id))
            {
                return NotFound();
            }
            var medicine=_medicineRepository.GetMedicine(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(medicine);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Medicine medicine)
        {
            if (medicine == null)
                return BadRequest(ModelState);

            if (!_medicineRepository.CreateMedicine(medicine))
            {
                ModelState.AddModelError("", "Error when adding medicine to database please try again latter");
                return StatusCode(500, ModelState);
            }
            return Ok("Succesfully created medicine");
        }
        [HttpPut("{id}")]
        public IActionResult UpdateMedicine(Medicine medicine)
        {
            if (medicine == null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!_medicineRepository.UpdateMedicine(medicine))
            {
                ModelState.AddModelError("", "Error when updataing medicine please try again latter");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteMedicine(int id)
        {
            if (!_medicineRepository.MedicineExist(id))
            {
                return NotFound();
            }
            var medicineToDelete = _medicineRepository.GetMedicine(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_medicineRepository.DeleteMedicine(medicineToDelete))
            {
                ModelState.AddModelError("", "Error when deleting medicine please try again latter");
            }
            return NoContent();
        }

    }
}
