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
        private readonly UserManager<ApiUser> _userManager;
        public PrescriptionController(IPrescriptionRepository prescriptionRepository, 
            IPrescriptionMedicineRepository prescriptionMedicineRepository, IMedicineRepository medicineRepository,
            UserManager<ApiUser> userManager)
        {
            _prescriptionRepository = prescriptionRepository;
            _prescriptionMedicineRepository = prescriptionMedicineRepository;
            _medicineRepository = medicineRepository;
            _userManager = userManager;
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
            var doctor = await _userManager.FindByIdAsync(prescription.DoctorId);
            if (doctor == null)
                return BadRequest(ModelState);
            var patient = await _userManager.FindByIdAsync(prescription.PatientId);
            if (patient== null)
                return BadRequest(ModelState);

            PrescriptionDisplayDTO prescriptionDisplayDTO = new PrescriptionDisplayDTO
            {
                Created_at = prescription.Created_at,
                ExpDate = prescription.ExpDate,
                DoctorFirstName = doctor.FirstName,
                DoctorLastName = doctor.LastName,
                PatientFirstName = patient.FirstName,
                PatientLastName = patient.LastName,
                PatientBirthDay = patient.BirthDay,
                PatientCity = patient.City,
                PatientStreet = patient.Street,
                PatientPostcode = patient.Postcode,
                PatientPhoneNumber = patient.PhoneNumber,
            };
            var prescriptionMedicines = await _prescriptionMedicineRepository.GetPrescriptionMedicineForPrescription(id);
            prescriptionDisplayDTO.PrescriptionMedicinesDTO = new List<PrescriptionMedicineDisplayDTO>();
            foreach(var prescriptionMedicine in prescriptionMedicines)
            {
                var medicine = await _medicineRepository.GetMedicine(prescriptionMedicine.MedicineId);
                PrescriptionMedicineDisplayDTO prescriptionMedicineDisplayDTO = new PrescriptionMedicineDisplayDTO
                {
                    Name = medicine.Name,
                    Strength = medicine.Strength,
                    Description = medicine.Description,
                    RouteOfAdministration = medicine.RouteOfAdministration,
                    Dosage = prescriptionMedicine.Dosage,
                    MedicineId = prescriptionMedicine.Id,
                    Quantity = prescriptionMedicine.Quantity
                };
                prescriptionDisplayDTO.PrescriptionMedicinesDTO.Add(prescriptionMedicineDisplayDTO);
            }
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

            var prescriptions =  await _prescriptionRepository.GetPrescriptionsForPatient(patientId);
            var patient = await _userManager.FindByIdAsync(patientId);
            if (patient == null)
                return NotFound();
            List<PrescriptionsListDisplayDTO> prescriptionsDisplayDTOs = new List<PrescriptionsListDisplayDTO>();
            foreach(var prescription in prescriptions)
            {
                var doctor = await _userManager.FindByIdAsync(prescription.DoctorId);
                if (doctor == null)
                    return BadRequest(ModelState);
                PrescriptionsListDisplayDTO prescriptionList = new PrescriptionsListDisplayDTO
                {
                    Id = prescription.Id,
                    Created_at = prescription.Created_at,
                    ExpDate = prescription.ExpDate,
                    PatientFirstName = patient.FirstName,
                    PatientLastName = patient.LastName,
                    DoctorFirstName = doctor.FirstName
                };
                prescriptionsDisplayDTOs.Add(prescriptionList);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(prescriptionsDisplayDTOs);
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
            if (doctor == null)
                return NotFound();
            var prescriptions = await _prescriptionRepository.GetPrescriptionsForDoctor(doctorId);
                
            List<PrescriptionsListDisplayDTO> prescriptionsDisplayDTOs = new List<PrescriptionsListDisplayDTO>();
            foreach (var prescription in prescriptions)
            {

                var patient = await _userManager.FindByIdAsync(prescription.PatientId);
                if (patient == null)
                    return BadRequest(ModelState);
                PrescriptionsListDisplayDTO prescriptionList = new PrescriptionsListDisplayDTO
                {
                    Id = prescription.Id,
                    Created_at = prescription.Created_at,
                    ExpDate = prescription.ExpDate,
                    PatientFirstName = patient.FirstName,
                    PatientLastName = patient.LastName,
                    DoctorFirstName = doctor.FirstName
                };
                prescriptionsDisplayDTOs.Add(prescriptionList);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(prescriptionsDisplayDTOs);
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
