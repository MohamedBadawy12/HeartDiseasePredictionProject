using Database.Entities;
using HearPrediction.Api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HearPrediction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalTestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public MedicalTestController(AppDbContext context, IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _userManager = userManager;
        }
        //Get MedicalTests by userId
        [Authorize("MedicalAnalyst")]
        [HttpGet("GetMedicalTestsByUserId")]
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var medicalTests = await _unitOfWork.medicalTest.GetMedicalTestsByUserId(userId, userRole);
            return Ok(medicalTests);
        }

        [Authorize("User")]
        [HttpGet("GetMedicalTestsByEmail")]
        public async Task<IActionResult> GetMedicalTests()
        {
            string PatientEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var medicalTests = await _unitOfWork.medicalTest.GetMedicalTestsByEmail(PatientEmail, userRole);
            return Ok(medicalTests);
        }

        [Authorize("MedicalAnalyst")]
        [AllowAnonymous]
        [HttpGet("GetMedicalDetails")]
        public async Task<IActionResult> MedicalTestDetails(int id)
        {
            var medicalTest = await _unitOfWork.medicalTest.GetMedicalTest(id);
            if (medicalTest == null)
                return NotFound($"MedicalTest with id {id} is not found");

            var medicalTestView = new PredictionDTO
            {
                Id = id,
                PatientName = medicalTest.PatientName,
                PatientEmail = medicalTest.PatientEmail,
                MedicalAnalystName = medicalTest.MedicalAnalystName,
                LabEmail = medicalTest.LabEmail,
                Date = medicalTest.Date,
                PatientSSN = medicalTest.PatientSSN,
                BloodPressureMedicine = medicalTest.BloodPressureMedicine,
                Prevalenthypertension = medicalTest.Prevalenthypertension,
                Age = medicalTest.Age,
                BMI = medicalTest.BMI,
                Diabetes = medicalTest.Diabetes,
                DiastolicBloodPressure = medicalTest.DiastolicBloodPressure,
                CholesterolLevel = medicalTest.CholesterolLevel,
                NumberOfCigarettes = medicalTest.NumberOfCigarettes,
                Gender = medicalTest.Gender,
                GlucoseLevel = medicalTest.GlucoseLevel,
                Smoking = medicalTest.Smoking,
                SystolicBloodPressure = medicalTest.SystolicBloodPressure,
                PrevalentStroke = medicalTest.PrevalentStroke,
            };
            return Ok(medicalTestView);
        }

        [Authorize("MedicalAnalyst")]
        [HttpPost("CreateMedicalTest")]
        public async Task<IActionResult> Create(int id, MedicalTestDto model)
        {
            var appointment = await _unitOfWork.labAppointment.GetAppointment(id);
            if (appointment == null)
                return NotFound($"Appointment with id {id} is not found");
            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string labEmail = User.FindFirstValue(ClaimTypes.Email);
            var medicalTest = new MedicalTest
            {
                UserId = userId,
                PatientName = $"{appointment.Patientt.FirstName} {appointment.Patientt.FirstName}",
                PatientEmail = appointment.Patientt.Email,
                MedicalAnalystName = model.MedicalAnalystName,
                LabEmail = labEmail,
                Date = DateTime.Now,
                PatientSSN = (long)appointment.Patientt.SSN,
                BloodPressureMedicine = model.BloodPressureMedicine,
                Prevalenthypertension = model.Prevalenthypertension,
                Age = model.Age,
                BMI = model.BMI,
                Diabetes = model.Diabetes,
                DiastolicBloodPressure = model.DiastolicBloodPressure,
                CholesterolLevel = model.CholesterolLevel,
                NumberOfCigarettes = model.NumberOfCigarettes,
                Gender = model.Gender,
                GlucoseLevel = model.GlucoseLevel,
                Smoking = model.Smoking,
                SystolicBloodPressure = model.SystolicBloodPressure,
                PrevalentStroke = model.PrevalentStroke,
            };
            await _unitOfWork.medicalTest.AddAsync(medicalTest);
            await _unitOfWork.Complete();
            return Ok(medicalTest);
        }

        [Authorize("MedicalAnalyst")]
        [HttpPut("MakePrediction")]
        public async Task<IActionResult> Prediction(int id, PredictionDTO model)
        {
            var medicalTest = await _unitOfWork.medicalTest.GetMedicalTest(id);
            if (medicalTest == null)
                return NotFound($"MedicalTest with id {id} is not found");


            medicalTest.Id = model.Id;
            medicalTest.PatientName = model.PatientName;
            medicalTest.PatientSSN = model.PatientSSN;
            medicalTest.PatientEmail = model.PatientEmail;
            medicalTest.Date = model.Date;
            medicalTest.LabEmail = model.LabEmail;
            medicalTest.Diabetes = model.Diabetes;
            medicalTest.Smoking = model.Smoking;
            medicalTest.CholesterolLevel = model.CholesterolLevel;
            medicalTest.BloodPressureMedicine = model.BloodPressureMedicine;
            medicalTest.BMI = model.BMI;
            medicalTest.Age = model.Age;
            medicalTest.Gender = model.Gender;
            medicalTest.DiastolicBloodPressure = model.DiastolicBloodPressure;
            medicalTest.Prevalenthypertension = model.Prevalenthypertension;
            medicalTest.PrevalentStroke = model.PrevalentStroke;
            medicalTest.NumberOfCigarettes = model.NumberOfCigarettes;
            medicalTest.SystolicBloodPressure = model.SystolicBloodPressure;
            medicalTest.GlucoseLevel = model.GlucoseLevel;
            medicalTest.Prediction = model.Prediction;
            medicalTest.Probability = model.Probability;

            await _unitOfWork.Complete();
            return Ok(medicalTest);
        }

        //Delete Medical Test 
        [Authorize(Roles = "MedicalAnalyst")]
        [HttpDelete("DeleteMedicalTest")]
        public IActionResult Delete(int id)
        {
            var isDeleted = _unitOfWork.medicalTest.Delete(id);
            return isDeleted ? Ok() : BadRequest();
        }
    }
}
