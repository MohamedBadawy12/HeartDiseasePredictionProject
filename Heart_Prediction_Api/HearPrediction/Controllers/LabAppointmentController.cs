using Database.Entities;
using HearPrediction.Api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HearPrediction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabAppointmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public LabAppointmentController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _context = context;
        }

        //Get All Appointments by User ID for patient
        [Authorize(Roles = "User")]
        [HttpGet()]
        public async Task<IActionResult> Index()
        {
            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.labAppointment.GetLabAppointmentsByPatientId(userId, userRole);
            return Ok(appointments);
        }


        //Get All Appointments by Email for doctor
        [Authorize(Roles = "MedicalAnalyst")]
        [HttpGet("GetLabAppointmentByEmail")]
        public async Task<IActionResult> GetLabAppointmentByEmail()
        {
            string labEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.labAppointment.GetLabAppointmentByEmail(labEmail, userRole);
            return Ok(appointments);
        }

        //Get All Accept and Cancel Appointments 
        [Authorize(Roles = "User")]
        [HttpGet("GetAcceptAndCancelAppointments")]
        public async Task<IActionResult> GetAcceptAndCancelLabAppointments()
        {
            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.labAppointment.GetAcceptAndCancelLabAppointment(userId, userRole);
            return Ok(appointments);
        }

        //Get All Accept Appointments By Medical Analyst
        [Authorize(Roles = "MedicalAnalyst")]
        [HttpGet("GetAcceptAppointmentsByMedicalAnalyst")]
        public async Task<IActionResult> GetAcceptAppointments()
        {
            string labEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.labAppointment.GetAcceptLabAppointmentByMedical(labEmail, userRole);
            return Ok(appointments);
        }

        [Authorize(Roles = "MedicalAnalyst")]
        [HttpGet("GetAcceptAppointment")]
        public async Task<IActionResult> GetAcceptLabAppointment(int id)
        {
            var appointment = await _unitOfWork.labAppointment.GetAcceptAppointment(id);
            if (appointment == null)
                return NotFound($"No appointment was found with Id: {id}");

            var app = new LabAppointmentDto
            {
                Id = id,
                PateintName = appointment.PateintName,
                PatientEmail = appointment.PatientEmail,
                PatinetImage = appointment.PatientImage,
                LabEmail = appointment.LabEmail,
                Date = appointment.Date,
                Time = appointment.Time,
                PatientSSN = appointment.PatientSSN,
                FirstName = appointment.Patientt.FirstName,
                LastName = appointment.Patientt.LastName,
                BirthDate = appointment.Patientt.BirthDate,
                Gender = appointment.Patientt.Gender,
                PhoneNumber = appointment.Patientt.PhoneNumber,
                LabId = appointment.LabId,
                Location = appointment.Lab.Location,
                Price = appointment.Lab.Price,
                Name = appointment.Lab.Name,
                LabPhoneNumber = appointment.Lab.PhoneNumber,
            };
            return Ok(app);
        }

        [Authorize(Roles = "MedicalAnalyst")]
        [HttpGet("GetAppointment")]
        public async Task<IActionResult> GetLabAppointment(int id)
        {
            var appointment = await _unitOfWork.labAppointment.GetAppointment(id);
            if (appointment == null)
                return NotFound($"No appointment was found with Id: {id}");

            var app = new LabAppointmentDto
            {
                Id = id,
                PateintName = appointment.PateintName,
                PatientEmail = appointment.PatientEmail,
                PatinetImage = appointment.PatientImage,
                LabEmail = appointment.LabEmail,
                Date = appointment.date,
                Time = appointment.Time,
                PatientSSN = appointment.PatientSSN,
                FirstName = appointment.Patientt.FirstName,
                LastName = appointment.Patientt.LastName,
                BirthDate = appointment.Patientt.BirthDate,
                Gender = appointment.Patientt.Gender,
                PhoneNumber = appointment.Patientt.PhoneNumber,
                LabId = appointment.LabId,
                Location = appointment.Lab.Location,
                Price = appointment.Lab.Price,
                Name = appointment.Lab.Name,
                LabPhoneNumber = appointment.Lab.PhoneNumber,
            };
            return Ok(app);
        }

        //Create Appointment
        [Authorize(Roles = "User")]
        [HttpPost("BookLabAppointment")]
        public async Task<IActionResult> Create(int id, BookLabAppointmentDto model)
        {
            var lab = await _unitOfWork.labs.GetLab(id);
            if (lab == null)
                return BadRequest("NotFound");

            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string patientEmail = User.FindFirstValue(ClaimTypes.Email);
            long patientSSN = (long)user.SSN;

            var dateWithTime = _context.LabAppointments.Where(x => x.date == model.Date && x.Time == model.Time &&
            x.LabEmail == lab.User.Email && x.PatientEmail == patientEmail).FirstOrDefault();
            if (dateWithTime != null)
                return BadRequest("This Date with this time is Booked");

            var appointment = new LabAppointment()
            {
                PatientID = userId,
                PateintName = model.PateintName,
                PatientEmail = patientEmail,
                PatientImage = user.ProfileImg,
                LabEmail = lab.User.Email,
                date = model.Date,
                Time = model.Time,
                PhoneNumber = model.PatientPhone,
                PatientSSN = patientSSN,
                IsAccepted = false,
                LabId = lab.Id,
            };

            await _unitOfWork.labAppointment.AddAsync(appointment);
            await _unitOfWork.Complete();
            return Ok(appointment);
        }

        //Edit Appointment
        [Authorize(Roles = "User")]
        [HttpPut("EditLabAppointment")]
        public async Task<IActionResult> Edit(int id, EditAppointmentDto model)
        {
            var appointment = await _unitOfWork.labAppointment.GetAppointment(id);
            if (appointment == null)
                return BadRequest("NotFound");

            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string patientEmail = User.FindFirstValue(ClaimTypes.Email);

            var dateWithTime = _context.LabAppointments.Where(x => x.date == model.Date && x.Time == model.Time &&
            x.LabEmail == appointment.LabEmail && x.PatientEmail == patientEmail).FirstOrDefault();
            if (dateWithTime != null)
                return BadRequest("This Date with this time is Booked");
            appointment.PatientID = userId;
            appointment.date = model.Date;
            appointment.Time = model.Time;
            appointment.PateintName = model.PateintName;
            appointment.PatientEmail = patientEmail;
            appointment.PatientImage = model.PatinetImage;
            appointment.PhoneNumber = user.PhoneNumber;
            appointment.IsAccepted = false;
            appointment.PatientSSN = model.PatientSSN;

            await _unitOfWork.Complete();
            return Ok(appointment);
        }


        //Accept Appointment
        [Authorize(Roles = "MedicalAnalyst")]
        [HttpPost("Accept")]
        public async Task<IActionResult> AcceptsLabAppointment(int id)
        {
            var appointment = await _unitOfWork.labAppointment.GetAppointment(id);
            if (appointment == null)
                return BadRequest("NotFound");
            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string labEmail = User.FindFirstValue(ClaimTypes.Email);

            appointment.PatientEmail = appointment.PatientEmail;
            appointment.date = appointment.date;
            appointment.Time = appointment.Time;
            appointment.PhoneNumber = appointment.PhoneNumber;
            appointment.LabEmail = appointment.LabEmail;
            appointment.LabId = appointment.LabId;
            appointment.PatientSSN = appointment.PatientSSN;
            appointment.PateintName = appointment.PateintName;
            appointment.PatientImage = appointment.PatientImage;
            appointment.IsAccepted = true;
            appointment.PatientID = appointment.PatientID;

            _context.LabAppointments.Update(appointment);
            await _unitOfWork.Complete();

            var acceptAppointment = new AcceptAndCancelLabAppointment
            {
                Date = appointment.date,
                Time = appointment.Time,
                PatientEmail = appointment.PatientEmail,
                LabEmail = labEmail,
                PateintName = appointment.PateintName,
                PatientSSN = appointment.PatientSSN,
                PatientImage = appointment.PatientImage,
                LabId = appointment.LabId,
                PhoneNumber = appointment.PhoneNumber,
                IsAccepted = true,
                PatientID = appointment.PatientID,
            };
            await _unitOfWork.labAppointment.AddAcceptOrCancelAsync(acceptAppointment);
            await _unitOfWork.Complete();

            var message = new Message
            {
                Messages = $"Your Appointment with date {appointment.date.ToString("dd MMMM yyyy")} and time {appointment.Time} is Accepted",
                Date = DateTime.Now,
                PatientEmail = appointment.PatientEmail,
                DoctorEmail = labEmail,
                DoctorId = userId,
            };
            await _unitOfWork.appointments.AddMessageAsync(message);
            await _unitOfWork.Complete();
            return Ok(message);
        }

        //Cancel Appointment by Doctor
        [Authorize(Roles = "MedicalAnalyst")]
        [HttpPost("CancelByMedicalAnalyst")]
        public async Task<IActionResult> CancelLabAppointment(int id)
        {
            var appointment = _unitOfWork.labAppointment.Get_LabAppointment(id);
            if (appointment != null)
            {
                string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userName == null)
                    return BadRequest("Register Or Login Please");
                var user = await _userManager.FindByNameAsync(userName);
                string userId = user.Id;
                string labEmail = User.FindFirstValue(ClaimTypes.Email);

                var cancelAppointment = new AcceptAndCancelLabAppointment
                {
                    Date = appointment.date,
                    Time = appointment.Time,
                    PatientEmail = appointment.PatientEmail,
                    LabEmail = labEmail,
                    PateintName = appointment.PateintName,
                    PatientSSN = appointment.PatientSSN,
                    PatientImage = appointment.PatientImage,
                    LabId = appointment.LabId,
                    PhoneNumber = appointment.PhoneNumber,
                    IsAccepted = false,
                    PatientID = appointment.PatientID,
                };
                await _unitOfWork.labAppointment.AddAcceptOrCancelAsync(cancelAppointment);
                await _unitOfWork.Complete();

                var message = new Message
                {
                    Messages = $"Sorry,Your Appointment with date {appointment.date.ToString("dd MMMM yyyy")} and time {appointment.Time} is Canceled because Doctor is busy in this time",
                    Date = DateTime.Now,
                    PatientEmail = appointment.PatientEmail,
                    DoctorEmail = labEmail,
                    DoctorId = userId,
                };
                await _unitOfWork.appointments.AddMessageAsync(message);
                await _unitOfWork.Complete();
                _unitOfWork.labAppointment.Canceled(id);
                await _unitOfWork.Complete();
                return Ok(message);
            }
            return BadRequest($"Appointment with {id} is Not Found");
        }

        //Cancel Appointment by Patient
        [Authorize(Roles = "User")]
        [HttpPost("CancelByPatient")]
        public async Task<IActionResult> CancelLabAppointmentByPatient(int id)
        {
            var appointment = _unitOfWork.labAppointment.Get_LabAppointment(id);
            if (appointment == null)
                return BadRequest($"Appointment with {id} is Not Found");
            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string patientEmail = User.FindFirstValue(ClaimTypes.Email);
            var cancelAppointment = new AcceptAndCancelLabAppointment
            {
                Date = appointment.date,
                Time = appointment.Time,
                PatientEmail = patientEmail,
                LabEmail = appointment.LabEmail,
                PateintName = appointment.PateintName,
                PatientSSN = appointment.PatientSSN,
                PatientImage = appointment.PatientImage,
                LabId = appointment.LabId,
                PhoneNumber = appointment.PhoneNumber,
                IsAccepted = false,
                PatientID = userId,
            };
            await _unitOfWork.labAppointment.AddAcceptOrCancelAsync(cancelAppointment);
            await _unitOfWork.Complete();

            _unitOfWork.labAppointment.Canceled(id);
            await _unitOfWork.Complete();
            return Ok(appointment);
        }
    }
}
