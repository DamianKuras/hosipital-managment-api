using AutoMapper;
using hosipital_managment_api.Data;
using hosipital_managment_api.Dto;
using hosipital_managment_api.Interface;
using hosipital_managment_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace hosipital_managment_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PrescriptionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        public PrescriptionController(IUnitOfWork unitOfWork,
            UserManager<ApiUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin,Doctor,Nurse,Pharmacist")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PrescriptionDisplayDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            
            var prescription = await _unitOfWork.PrescriptionRepository.FindOne(p=>p.Id==id,new List<string> { "Doctor","Patient"});
            if (prescription == null)
            {
                return NotFound();
            }
            var prescriptionDisplayDTO = _mapper.Map<PrescriptionDisplayDTO>(prescription);

            var prescriptionMedicines = await _unitOfWork.PrescriptionMedicineRepository.FindAll(
                prescriptionMedicine => prescriptionMedicine.PrescriptionId == prescription.Id, new List<string>{ "Medicine"});
            prescriptionDisplayDTO.PrescriptionMedicinesDTO = _mapper.Map<IEnumerable<PrescriptionMedicineDisplayDTO>>(prescriptionMedicines);
            return Ok(prescriptionDisplayDTO);
        }
        [Authorize(Roles = "Admin,Doctor,Nurse,Pharmacist")]
        [HttpGet("Patient/{patientId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PrescriptionsListDisplayDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patient([FromQuery] PagingParameters pagingParameters,string patientId)
        {
            var patient = await _userManager.FindByIdAsync(patientId);
            if (patient == null)
                return NotFound();
            var prescriptions = await _unitOfWork.PrescriptionRepository.FindPaged(pagingParameters,
                p=>p.PatientId==patientId, new List<string> { "Doctor", "Patient" });
            var prescriptionsListDisplayDTO = _mapper.Map<IEnumerable<PrescriptionsListDisplayDTO>>(prescriptions);
            return Ok(prescriptionsListDisplayDTO);
        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("Doctor/{doctorId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PrescriptionsListDisplayDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Doctor([FromQuery] PagingParameters pagingParameters,string doctorId)
        {
            var doctor = await _userManager.FindByIdAsync(doctorId);
            var isDoctor = await _userManager.IsInRoleAsync(doctor, "Doctor");
            if (doctor == null || !isDoctor)
                return NotFound();
            var prescriptions = await _unitOfWork.PrescriptionRepository.FindPaged(pagingParameters,
                p =>p.DoctorId == doctorId, new List<string> { "Doctor", "Patient" });
            var prescriptionsListDisplayDTO = _mapper.Map<IEnumerable<PrescriptionsListDisplayDTO>>(prescriptions);
            return Ok(prescriptionsListDisplayDTO);
        }

        [Authorize(Roles = "Admin,Doctor,Nurse,Pharmacist")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(PrescriptionDTO prescriptionDTO)
        {
            if (prescriptionDTO == null)
                return BadRequest(ModelState);

            var doctor = await _userManager.FindByIdAsync(prescriptionDTO.DoctorId);
            var isDoctor = await _userManager.IsInRoleAsync(doctor, "Doctor");
            if (doctor == null || !isDoctor)
                return BadRequest(ModelState);
            var patient = await _userManager.FindByIdAsync(prescriptionDTO.PatientId);
            if (patient == null)
                return BadRequest(ModelState);
            Prescription prescription = new Prescription
            {
                Patient = patient,
                Doctor = doctor,
                Created_at = DateTime.Now,
                ExpDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
            };

            _unitOfWork.PrescriptionRepository.Add(prescription);
            foreach (var prescriptionMedicineDTO in prescriptionDTO.PrescriptionMedicinesDTO)
            {
                if (prescriptionMedicineDTO == null)
                    return BadRequest(ModelState);
                PrescriptionMedicine prescriptionMedicine = new PrescriptionMedicine
                {
                    Prescription = prescription,
                    MedicineId = prescriptionMedicineDTO.MedicineId,
                    Quantity = prescriptionMedicineDTO.Quantity,
                    Dosage = prescriptionMedicineDTO.Dosage
                };
                _unitOfWork.PrescriptionMedicineRepository.Add(prescriptionMedicine);
            }
            if(!await _unitOfWork.Save())
            {
                ModelState.AddModelError("", "Error when adding prescription to database please try again latter");
                return StatusCode(500, ModelState);
            }
            return Ok();
        }
    }
}
