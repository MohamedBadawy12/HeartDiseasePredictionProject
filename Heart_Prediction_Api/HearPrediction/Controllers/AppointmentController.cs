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
    public class AppointmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public AppointmentController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, AppDbContext context)
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
            var appointments = await _unitOfWork.appointments.GetAppointmentByUserId(userId, userRole);
            return Ok(appointments);
        }

        //Get All Appointments by Email for doctor
        [Authorize(Roles = "Doctor")]
        [HttpGet("GetAppointmentByEmail")]
        public async Task<IActionResult> GetAppointmentByEmail()
        {
            string doctorEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.appointments.GetAppointmentByEmail(doctorEmail, userRole);
            return Ok(appointments);
        }

        //Get All Appointments by Email
        [Authorize(Roles = "User")]
        [HttpGet("GetMessagetByEmail")]
        public async Task<IActionResult> GetMessagetByEmail()
        {
            string patientEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var messages = await _unitOfWork.appointments.GetMessageByEmail(patientEmail, userRole);
            return Ok(messages);
        }

        //Get All Accept and Cancel Appointments 
        [Authorize(Roles = "User")]
        [HttpGet("GetAcceptAndCancelAppointments")]
        public async Task<IActionResult> GetAcceptAndCancelAppointments()
        {
            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.appointments.GetAcceptAndCancelAppointment(userId, userRole);
            return Ok(appointments);
        }

        //Get All Accept Appointments By Doctor
        [Authorize(Roles = "Doctor")]
        [HttpGet("GetAcceptAppointments")]
        public async Task<IActionResult> GetAcceptAppointments()
        {
            string doctorEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.appointments.GetAcceptAppointmentByDoctor(doctorEmail, userRole);
            return Ok(appointments);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("GetAcceptAppointment")]
        public async Task<IActionResult> GetAcceptAppointment(int id)
        {
            var appointment = await _unitOfWork.appointments.GetAcceptAppointment(id);
            if (appointment == null)
                return NotFound($"No appointment was found with Id: {id}");

            var app = new AppointmentDto
            {
                Id = id,
                PateintName = appointment.PateintName,
                PatientEmail = appointment.PatientEmail,
                DoctorEmail = appointment.DoctorEmail,
                PatientImage = appointment.PatientImage,
                PatientID = appointment.PatientID,
                Date = appointment.Date,
                Time = appointment.Time,
                PatientSSN = appointment.PatientSSN,
                PateintFirstName = appointment.Patientt.FirstName,
                PatientLastName = appointment.Patientt.LastName,
                PatientBirthDate = appointment.Patientt.BirthDate,
                PatientGender = appointment.Patientt.Gender,
                PatientPhoneNumber = appointment.Patientt.PhoneNumber,
                DoctorId = appointment.DoctorId,
                Location = appointment.Doctor.Location,
                Price = appointment.Doctor.Price,
                DoctorName = appointment.Doctor.Name,
            };
            return Ok(app);
        }


        [Authorize(Roles = "Doctor")]
        [HttpGet("GetAppointment")]
        public async Task<IActionResult> GetAppointment(int id)
        {
            var appointment = await _unitOfWork.appointments.GetAppointment(id);
            if (appointment == null)
                return NotFound($"No appointment was found with Id: {id}");

            var app = new AppointmentDto
            {
                Id = id,
                PateintName = appointment.PateintName,
                PatientEmail = appointment.PatientEmail,
                PatientImage = appointment.PatientImage,
                DoctorEmail = appointment.DoctorEmail,
                PatientID = appointment.PatientID,
                Date = appointment.date,
                Time = appointment.Time,
                PatientSSN = appointment.PatientSSN,
                PateintFirstName = appointment.Patientt.FirstName,
                PatientLastName = appointment.Patientt.LastName,
                PatientBirthDate = appointment.Patientt.BirthDate,
                PatientGender = appointment.Patientt.Gender,
                PatientPhoneNumber = appointment.Patientt.PhoneNumber,
                DoctorId = appointment.DoctorId,
                Location = appointment.Doctor.Location,
                Price = appointment.Doctor.Price,
                DoctorName = appointment.Doctor.Name,
            };
            return Ok(app);
        }

        //Create Appointment
        [Authorize(Roles = "User")]
        [HttpPost("BookAppointment")]
        public async Task<IActionResult> Create(int id, BookAppointmentDto model)
        {
            var doctor = await _unitOfWork.Doctors.GetDoctor(id);
            if (doctor == null)
                return BadRequest("NotFound");

            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string patientEmail = User.FindFirstValue(ClaimTypes.Email);
            long patientSSN = (long)user.SSN;

            var dateWithTime = _context.Appointments.Where(x => x.date == model.Date && x.Time == model.Time &&
            x.DoctorEmail == doctor.User.Email && x.PatientEmail == patientEmail).FirstOrDefault();
            if (dateWithTime != null)
                return BadRequest("This Date with this time is Booked");

            var appointment = new Appointment()
            {
                PatientID = userId,
                PateintName = model.PateintName,
                PatientEmail = patientEmail,
                PatientImage = user.ProfileImg,
                DoctorEmail = doctor.User.Email,
                date = model.Date,
                Time = model.Time,
                PhoneNumber = model.PatientPhone,
                PatientSSN = patientSSN,
                IsAccepted = false,
                DoctorId = doctor.Id,
            };

            await _unitOfWork.appointments.AddAsync(appointment);
            await _unitOfWork.Complete();
            return Ok(appointment);
        }

        //Edit Appointment
        [Authorize(Roles = "User")]
        [HttpPut("EditAppointment")]
        public async Task<IActionResult> Edit(int id, EditAppointmentDto model)
        {
            var appointment = await _unitOfWork.appointments.GetAppointment(id);
            if (appointment == null)
                return BadRequest("NotFound");

            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string patientEmail = User.FindFirstValue(ClaimTypes.Email);
            //long patientSSN = (long)user.SSN;

            var dateWithTime = _context.Appointments.Where(x => x.date == model.Date && x.Time == model.Time &&
            x.DoctorEmail == appointment.DoctorEmail && x.PatientEmail == patientEmail).FirstOrDefault();
            if (dateWithTime != null)
                return BadRequest("This Date with this time is Booked");
            appointment.PatientID = userId;
            appointment.date = model.Date;
            appointment.Time = model.Time;
            appointment.PateintName = model.PateintName;
            appointment.PatientEmail = patientEmail;
            appointment.PhoneNumber = user.PhoneNumber;
            appointment.PatientImage = model.PatinetImage;
            appointment.IsAccepted = false;
            //appointment.DoctorId = model.DoctorId;
            appointment.PatientSSN = model.PatientSSN;

            await _unitOfWork.Complete();
            return Ok(appointment);
        }

        //Accept Appointment
        [Authorize(Roles = "Doctor")]
        [HttpPost("Accept")]
        public async Task<IActionResult> AcceptsAppointment(int id)
        {
            var appointment = await _unitOfWork.appointments.GetAppointment(id);
            if (appointment == null)
                return BadRequest("NotFound");
            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string doctorEmail = User.FindFirstValue(ClaimTypes.Email);

            appointment.PatientEmail = appointment.PatientEmail;
            appointment.date = appointment.date;
            appointment.Time = appointment.Time;
            appointment.PhoneNumber = appointment.PhoneNumber;
            appointment.DoctorEmail = appointment.DoctorEmail;
            appointment.DoctorId = appointment.DoctorId;
            appointment.PatientSSN = appointment.PatientSSN;
            appointment.PateintName = appointment.PateintName;
            appointment.PatientImage = appointment.PatientImage;
            appointment.IsAccepted = true;
            appointment.PatientID = appointment.PatientID;

            _context.Appointments.Update(appointment);
            await _unitOfWork.Complete();

            var acceptAppointment = new AcceptAndCancelAppointment
            {
                Date = appointment.date,
                Time = appointment.Time,
                PatientEmail = appointment.PatientEmail,
                DoctorEmail = doctorEmail,
                PateintName = appointment.PateintName,
                PatientSSN = appointment.PatientSSN,
                PatientImage = appointment.PatientImage,
                DoctorId = appointment.DoctorId,
                PhoneNumber = appointment.PhoneNumber,
                IsAccepted = true,
                PatientID = appointment.PatientID,
            };
            await _unitOfWork.appointments.AddAcceptOrCancelAsync(acceptAppointment);
            await _unitOfWork.Complete();

            var message = new Message
            {
                Messages = $"Your Appointment with date {appointment.date.ToString("dd MMMM yyyy")} and time {appointment.Time} is Accepted",
                Date = DateTime.Now,
                PatientEmail = appointment.PatientEmail,
                DoctorEmail = doctorEmail,
                DoctorId = userId,
            };
            await _unitOfWork.appointments.AddMessageAsync(message);
            await _unitOfWork.Complete();
            return Ok(message);
        }

        //Cancel Appointment by Doctor
        [Authorize(Roles = "Doctor")]
        [HttpPost("CancelByDoctor")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var appointment = _unitOfWork.appointments.Get_Appointment(id);
            if (appointment != null)
            {
                string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userName == null)
                    return BadRequest("Register Or Login Please");
                var user = await _userManager.FindByNameAsync(userName);
                string userId = user.Id;
                string doctorEmail = User.FindFirstValue(ClaimTypes.Email);

                var cancelAppointment = new AcceptAndCancelAppointment
                {
                    Date = appointment.date,
                    Time = appointment.Time,
                    PatientEmail = appointment.PatientEmail,
                    DoctorEmail = doctorEmail,
                    PateintName = appointment.PateintName,
                    PatientSSN = appointment.PatientSSN,
                    DoctorId = appointment.DoctorId,
                    PhoneNumber = appointment.PhoneNumber,
                    PatientImage = appointment.PatientImage,
                    IsAccepted = false,
                    PatientID = appointment.PatientID,
                };
                await _unitOfWork.appointments.AddAcceptOrCancelAsync(cancelAppointment);
                await _unitOfWork.Complete();

                var message = new Message
                {
                    Messages = $"Sorry,Your Appointment with date {appointment.date.ToString("dd MMMM yyyy")} and time {appointment.Time} is Canceled because Doctor is busy in this time",
                    Date = DateTime.Now,
                    PatientEmail = appointment.PatientEmail,
                    DoctorEmail = doctorEmail,
                    DoctorId = userId,
                };
                await _unitOfWork.appointments.AddMessageAsync(message);
                await _unitOfWork.Complete();
                _unitOfWork.appointments.Cancel(appointment);
                await _unitOfWork.Complete();
                return Ok(message);
            }
            return BadRequest($"Appointment with {id} is Not Found");
        }

        //Cancel Appointment by Patient
        [Authorize(Roles = "User")]
        [HttpPost("CancelByPatient")]
        public async Task<IActionResult> CancelAppointmentByPatient(int id)
        {
            var appointment = _unitOfWork.appointments.Get_Appointment(id);
            if (appointment == null)
                return BadRequest($"Appointment with {id} is Not Found");
            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByNameAsync(userName);
            string userId = user.Id;
            string patientEmail = User.FindFirstValue(ClaimTypes.Email);
            var cancelAppointment = new AcceptAndCancelAppointment
            {
                Date = appointment.date,
                Time = appointment.Time,
                PatientEmail = patientEmail,
                DoctorEmail = appointment.DoctorEmail,
                PateintName = appointment.PateintName,
                PatientSSN = appointment.PatientSSN,
                PatientImage = appointment.PatientImage,
                DoctorId = appointment.DoctorId,
                PhoneNumber = appointment.PhoneNumber,
                IsAccepted = false,
                PatientID = userId,
            };
            await _unitOfWork.appointments.AddAcceptOrCancelAsync(cancelAppointment);
            await _unitOfWork.Complete();

            _unitOfWork.appointments.Cancel(appointment);
            await _unitOfWork.Complete();
            return Ok(appointment);
        }
    }
}
