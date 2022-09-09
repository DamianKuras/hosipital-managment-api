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
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IPrescriptionMedicineRepository _prescriptionMedicineRepository;
        private readonly IMedicineRepository _medicineRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        public PrescriptionController(IPrescriptionRepository prescriptionRepository, 
            IPrescriptionMedicineRepository prescriptionMedicineRepository, IMedicineRepository medicineRepository,
            UserManager<ApiUser> userManager, IMapper mapper)
        {
            _prescriptionRepository = prescriptionRepository;
            _prescriptionMedicineRepository = prescriptionMedicineRepository;
            _medicineRepository = medicineRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin,Doctor,Nurse,Pharmacist")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PrescriptionDisplayDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            if (!await _prescriptionRepository.PrescriptionExist(id)) 
            {
                return NotFound();
            }
            var prescription = await _prescriptionRepository.GetPrescription(id);
            var prescriptionDisplayDTO =_mapper.Map<PrescriptionDisplayDTO>(prescription);
            var prescriptionMedicines = await _prescriptionMedicineRepository.GetPrescriptionMedicineForPrescription(id);
            prescriptionDisplayDTO.PrescriptionMedicinesDTO = _mapper.Map<IEnumerable<PrescriptionMedicineDisplayDTO>>(prescriptionMedicines);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(prescriptionDisplayDTO);
        }
        [Authorize(Roles = "Admin,Doctor,Nurse,Pharmacist")]
        [HttpGet("Patient/{patientId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PrescriptionsListDisplayDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patient(string patientId)
        {
            var patient = await _userManager.FindByIdAsync(patientId);
            if (patient == null)
                return NotFound();
            var prescriptions = await _prescriptionRepository.GetPrescriptionsForPatient(patientId);
            var prescriptionsListDisplayDTO = _mapper.Map<IEnumerable<PrescriptionsListDisplayDTO>>(prescriptions);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(prescriptionsListDisplayDTO);
        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("Doctor/{doctorId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PrescriptionsListDisplayDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Doctor(string doctorId)
        {
            var doctor = await _userManager.FindByIdAsync(doctorId);
            var isDoctor = await _userManager.IsInRoleAsync(doctor, "Doctor");
            if (doctor == null || !isDoctor)
                return NotFound();
            var prescriptions = await _prescriptionRepository.GetPrescriptionsForDoctor(doctorId);
            var prescriptionsListDisplayDTO = _mapper.Map<IEnumerable<PrescriptionsListDisplayDTO>>(prescriptions);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(prescriptionsListDisplayDTO);
        }

        [Authorize(Roles = "Admin,Doctor,Nurse,Pharmacist")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkObjectResult))]
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
            if(patient == null) 
                return BadRequest(ModelState);
            Prescription prescription = new Prescription
            {
                Patient = patient,
                Doctor = doctor,
                Created_at = DateTime.Now,
                ExpDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(6)),
            };
            if (!await _prescriptionRepository.CreatePrescription(prescription))
            {
                ModelState.AddModelError("", "Error when adding prescription to database please try again latter");
                return StatusCode(500, ModelState);
            }
            foreach (var prescriptionMedicineDTO in prescriptionDTO.PrescriptionMedicinesDTO)
            {
                if(prescriptionMedicineDTO == null)
                    return BadRequest(ModelState);
                PrescriptionMedicine prescriptionMedicine = new PrescriptionMedicine
                {
                    PrescriptionId = prescription.Id,
                    MedicineId = prescriptionMedicineDTO.MedicineId,
                    Quantity = prescriptionMedicineDTO.Quantity,
                    Dosage = prescriptionMedicineDTO.Dosage
                };
                if (!await _prescriptionMedicineRepository.CreatePrescriptionMedicine(prescriptionMedicine))
                {
                    ModelState.AddModelError("", "Error when adding prescriptionMedicine to database please try again latter");
                    return StatusCode(500, ModelState);
                }

            }
            return Ok("Succesfully created prescription");
        }
    }
}
