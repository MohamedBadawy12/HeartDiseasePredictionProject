using Database.Entities;
using HearPrediction.Api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HearPrediction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public PrescriptionController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _context = context;
        }

        //Get All Prescriptions By UserID for doctor
        [Authorize(Roles = "Doctor")]
        [HttpGet()]
        public async Task<IActionResult> GetAllPrescriptionsByUserId()
        {
            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var prescriptions = await _unitOfWork.prescriptions.GetPrescriptionByUserId(userId, userRole);
            return Ok(prescriptions);
        }

        //Get All Prescriptions By Email for patient
        [Authorize(Roles = "User")]
        [HttpGet("GetAllPrescriptionsByEmail")]
        public async Task<IActionResult> GetAllPrescriptionsByEmail()
        {
            string PatientEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var prescriptions = await _unitOfWork.prescriptions.GetPrescriptionByEmail(PatientEmail, userRole);
            return Ok(prescriptions);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("GetPatientPrescriptions")]
        public async Task<IActionResult> GetPatientPrescriptions(int id)
        {
            var appointment = await _unitOfWork.appointments.GetAppointment(id);
            if (appointment == null)
                return NotFound($"No appointment was found with Id: {id}");

            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");

            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string doctorEmail = User.FindFirstValue(ClaimTypes.Email);

            var prescriptions = await _context.Prescriptions.Where(x => x.PatientSSN == appointment.PatientSSN &&
            x.DoctorEmail == doctorEmail && x.ApDoctorId == userId).Include(x => x.Doctorr).ToListAsync();
            if (prescriptions == null)
                return NotFound($"No prescription was found with Id: {id}");
            return Ok(prescriptions);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("GetAcceptPatientPrescriptions")]
        public async Task<IActionResult> GetAcceptPatientPrescriptions(int id)
        {
            var appointment = await _unitOfWork.appointments.GetAcceptAppointment(id);
            if (appointment == null)
                return NotFound($"No appointment was found with Id: {id}");

            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");

            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string doctorEmail = User.FindFirstValue(ClaimTypes.Email);

            var prescriptions = await _context.Prescriptions.Where(x => x.PatientSSN == appointment.PatientSSN &&
            x.DoctorEmail == doctorEmail && x.ApDoctorId == userId).Include(x => x.Doctorr).ToListAsync();
            if (prescriptions == null)
                return NotFound($"No prescription was found with Id: {id}");
            return Ok(prescriptions);
        }

        //Get Details of Prescription by id from db
        [HttpGet("GetPrescriptionById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPrescriptionDetails(int id)
        {
            var prescription = await _unitOfWork.prescriptions.GetPrescription(id);
            if (prescription == null)
                return NotFound($"No prescription was found with Id: {id}");

            var PrescriptionrDetail = new PrescriptionFormDTO
            {
                Id = prescription.Id,
                MedicineName = prescription.MedicineName,
                PatientSSN = prescription.PatientSSN,
                Date = prescription.date,
                ApDoctorId = prescription.ApDoctorId,
                DoctorEmail = prescription.DoctorEmail,
                PatientEmail = prescription.PatientEmail,
                DoctorId = prescription.DoctorId,
                DoctorName = prescription.Doctor.Name,
            };
            return Ok(PrescriptionrDetail);
        }


        //Search for Prescription
        [HttpGet("Search")]
        public async Task<IActionResult> SearchForPrescription([FromQuery] long search)
        {
            try
            {
                var result = await _unitOfWork.prescriptions.FilterPrescriptions(search);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in search for data");
            }
        }

        //Create a Prescription and save it in db
        [Authorize(Roles = "Doctor")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PrescriptionDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var doctor = await _unitOfWork.Doctors.GetDoctor(model.DoctorId);
            if (doctor == null)
                return BadRequest("Doctor not Found");

            var patient = await _unitOfWork.Patients.GetPatient(model.PatientSSN);
            if (patient == null)
                return BadRequest("Patient not Found");

            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string doctorEmail = User.FindFirstValue(ClaimTypes.Email);
            //string doctorEmail = user.Email;
            var prescription = new Prescription()
            {
                DoctorId = model.DoctorId,
                PatientSSN = model.PatientSSN,
                date = model.date,
                MedicineName = model.MedicineName,
                DoctorEmail = doctorEmail,
                PatientEmail = model.PatientEmail,
                ApDoctorId = userId,
            };
            await _unitOfWork.prescriptions.AddAsync(prescription);
            await _unitOfWork.Complete();
            return Ok(prescription);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("CreatePrescription")]
        public async Task<IActionResult> CreatePresciption(int id, [FromBody] CreatePrescriptionDto model)
        {
            var appointment = await _unitOfWork.appointments.GetAppointment(id);
            if (appointment == null)
                return NotFound();

            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");

            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string doctorEmail = User.FindFirstValue(ClaimTypes.Email);
            //string doctorEmail = user.Email;
            var prescription = new Prescription
            {
                ApDoctorId = userId,
                MedicineName = model.MedicineName,
                date = DateTime.Now,
                DoctorEmail = doctorEmail,
                PatientEmail = appointment.Patientt.Email,
                DoctorId = appointment.DoctorId,
                PatientSSN = appointment.PatientSSN,
            };
            await _unitOfWork.prescriptions.AddAsync(prescription);
            await _unitOfWork.Complete();
            return Ok(prescription);
        }

        //Edit the Prescription
        [Authorize(Roles = "Doctor")]
        [HttpPut("EditPrescription")]
        public async Task<IActionResult> EditPrescription(int id, [FromBody] PrescriptionDto model)
        {
            try
            {
                var prescription = await _unitOfWork.prescriptions.GetPrescription(id);
                if (prescription == null)
                    return NotFound($"No prescription was found with Id: {id}");

                string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userName == null)
                    return BadRequest("Register Or Login Please");
                var user = await _userManager.FindByNameAsync(userName);
                string userId = user.Id;
                string doctorEmail = User.FindFirstValue(ClaimTypes.Email);
                //string doctorEmail = user.Email;
                prescription.DoctorId = model.DoctorId;
                prescription.PatientSSN = model.PatientSSN;
                prescription.date = model.date;
                prescription.MedicineName = model.MedicineName;
                prescription.ApDoctorId = userId;
                prescription.DoctorEmail = doctorEmail;
                prescription.PatientEmail = model.PatientEmail;
                await _unitOfWork.Complete();
                return Ok(prescription);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "  " + ex.StackTrace);
            }
        }

        //Delete the prescription
        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var prescription = _unitOfWork.prescriptions.Get_Prescription(id);
            if (prescription == null)
                return NotFound($"No prescription was found with Id: {id}");
            try
            {
                _unitOfWork.prescriptions.Remove(prescription);
                await _unitOfWork.Complete();
                return Ok(prescription);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }
    }
}
