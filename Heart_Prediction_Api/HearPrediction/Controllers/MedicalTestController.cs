using Database.Entities;
using HearPrediction.Api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Linq;
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
        [Authorize(Roles = "MedicalAnalyst")]
        [HttpGet("GetMedicalTestsByUserId")]
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var medicalTests = await _unitOfWork.medicalTest.GetMedicalTestsByUserId(userId, userRole);
            return Ok(medicalTests);
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetMedicalTestsByEmail")]
        public async Task<IActionResult> GetMedicalTests()
        {
            string PatientEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var medicalTests = await _unitOfWork.medicalTest.GetMedicalTestsByEmail(PatientEmail, userRole);
            return Ok(medicalTests);
        }

        //[Authorize(Roles = "MedicalAnalyst")]
        [AllowAnonymous]
        [HttpGet("GetMedicalDetails")]
        public async Task<IActionResult> MedicalTestDetails(int id)
        {
            var medicalTest = await _unitOfWork.medicalTest.GetMedicalTest(id);
            if (medicalTest == null)
                return NotFound($"MedicalTest with id {id} is not found");

            var medicalTestView = new PredictionDTO
            {
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
                HeartRate = medicalTest.HeartRate,
                Prediction = medicalTest.Prediction,
                Probability = medicalTest.Probability,
            };
            return Ok(medicalTestView);
        }

        [Authorize(Roles = "MedicalAnalyst")]
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
                PatientName = $"{appointment.Patientt.FirstName} {appointment.Patientt.LastName}",
                PatientEmail = appointment.Patientt.Email,
                MedicalAnalystName = model.MedicalAnalystName,
                LabEmail = labEmail,
                Date = model.Date,
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
                HeartRate = model.HeartRate,
            };
            await _unitOfWork.medicalTest.AddAsync(medicalTest);
            await _unitOfWork.Complete();
            return Ok(medicalTest);
        }


        [Authorize(Roles = "MedicalAnalyst")]
        [HttpGet("GetPatientMedicalTest")]
        public async Task<IActionResult> GetPatientPrescriptions(int id)
        {
            var appointment = await _unitOfWork.labAppointment.GetAppointment(id);
            if (appointment == null)
                return NotFound($"No appointment was found with Id: {id}");

            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");

            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string labEmail = User.FindFirstValue(ClaimTypes.Email);

            var medicalTets = await _context.MedicalTests.Where(x => x.PatientSSN == appointment.PatientSSN &&
            x.LabEmail == labEmail && x.UserId == userId).Include(x => x.Labb).ToListAsync();
            if (medicalTets == null)
                return NotFound($"No medical Tets was found with Id: {id}");
            return Ok(medicalTets);
        }

        [AllowAnonymous]
        [HttpGet("GetMedicalTestDetails")]
        public async Task<IActionResult> MedicalTest(int id)
        {
            var medicalTest = await _unitOfWork.medicalTest.GetMedicalTest(id);
            if (medicalTest == null)
                return NotFound($"MedicalTest with id {id} is not found");

            var medicalTestView = new PredictionDetailsDTO
            {
                BPMeds = (int)medicalTest.BloodPressureMedicine,
                PrevalentHyp = (int)medicalTest.Prevalenthypertension,
                Age = (int)medicalTest.Age,
                BMI = (float)medicalTest.BMI,
                Diabetes = (int)medicalTest.Diabetes,
                DiaBP = (float)medicalTest.DiastolicBloodPressure,
                Cholesterol = (int)medicalTest.CholesterolLevel,
                CigsPerDay = (int)medicalTest.NumberOfCigarettes,
                Sex = (int)medicalTest.Gender,
                Glucose = (int)medicalTest.GlucoseLevel,
                Smoker = (int)medicalTest.Smoking,
                SysBP = (float)medicalTest.SystolicBloodPressure,
                HeartRate = (int)medicalTest.HeartRate,
                PrevalentStroke = (int)medicalTest.PrevalentStroke,
                prediction = (int)medicalTest.Prediction,
                probability = (int)medicalTest.Probability,
            };
            return Ok(medicalTestView);
        }

        [Authorize(Roles = "User")]
        [HttpPut("MakePrediction")]
        public async Task<IActionResult> Prediction(int id, PredictionDetailsDTO model)
        {
            var medicalTest = await _unitOfWork.medicalTest.GetMedicalTest(id);
            if (medicalTest == null)
                return NotFound($"MedicalTest with id {id} is not found");

            medicalTest.Diabetes = model.Diabetes;
            medicalTest.Smoking = model.Smoker;
            medicalTest.CholesterolLevel = model.Cholesterol;
            medicalTest.BloodPressureMedicine = model.BPMeds;
            medicalTest.BMI = model.BMI;
            medicalTest.Age = model.Age;
            medicalTest.Gender = (Database.Enums.Gender)model.Sex;
            medicalTest.DiastolicBloodPressure = model.DiaBP;
            medicalTest.Prevalenthypertension = model.PrevalentHyp;
            medicalTest.PrevalentStroke = model.PrevalentStroke;
            medicalTest.NumberOfCigarettes = model.CigsPerDay;
            medicalTest.SystolicBloodPressure = model.SysBP;
            medicalTest.HeartRate = model.HeartRate;
            medicalTest.GlucoseLevel = model.Glucose;
            medicalTest.Prediction = model.prediction;
            medicalTest.Probability = model.probability;
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
