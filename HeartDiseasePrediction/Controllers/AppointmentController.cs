using Database.Entities;
using HeartDiseasePrediction.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Repositories;
using Repositories.Interfaces;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HeartDiseasePrediction.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public AppointmentController(IUnitOfWork unitOfWork, IToastNotification toastNotification,
            IFileService fileRepository, AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;
            _context = context;
            _userManager = userManager;
        }

        //Get All Appointments by User ID
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.appointments.GetAppointmentsByPatientId(userId, userRole);
            int totalRecords = appointments.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            appointments = appointments.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(appointments);
        }

        //Get All Appointments by Email
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetAppointmentByEmail(int currentPage = 1)
        {
            string doctorEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.appointments.GetWaitingAppointmentByEmail(doctorEmail, userRole);
            int totalRecords = appointments.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            appointments = appointments.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(appointments);
        }

        //Get All messages by Email
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetMessagetByEmail(int currentPage = 1)
        {
            string patientEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var messages = await _unitOfWork.appointments.GetMessageByEmail(patientEmail, userRole);
            int totalRecords = messages.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            messages = messages.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(messages);
        }

        //Search for message by Email
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> GetMessagetByEmail(DateTime? date, int currentPage = 1)
        {
            string patientEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var messages = await _unitOfWork.appointments.SearchMessages(patientEmail, userRole, date);
            int totalRecords = messages.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            messages = messages.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(messages);
        }

        //Get All Accept and Cancel Appointments By Patient
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAcceptAndCancelAppointments(int currentPage = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.appointments.GetAcceptAndCancelAppointment(userId, userRole);
            int totalRecords = appointments.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            appointments = appointments.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(appointments);
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> GetAcceptAndCancelAppointments(DateTime? date, int currentPage = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.appointments.SearchAcceptAndCancelAppointment(userId, userRole, date);
            int totalRecords = appointments.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            appointments = appointments.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(appointments);
        }

        //Get All Accept Appointments By Doctor
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetAcceptAppointments(int currentPage = 1)
        {
            string doctorEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.appointments.GetAcceptAppointmentByDoctor(doctorEmail, userRole);
            int totalRecords = appointments.Count();
            int pageSize = 20;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            appointments = appointments.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(appointments);
        }
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> GetAcceptAppointments(DateTime? date, long? ssn, int currentPage = 1)
        {
            string doctorEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.appointments.SearchAcceptAppointments(doctorEmail, userRole, date, ssn);
            int totalRecords = appointments.Count();
            int pageSize = 20;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            appointments = appointments.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(appointments);
        }

        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetAcceptAppointment(int id)
        {
            var appointment = await _unitOfWork.appointments.GetAcceptAppointment(id);
            if (appointment == null)
                return View("NotFound");

            var app = new AppointmentVM
            {
                Id = id,
                PateintName = appointment.PateintName,
                PatientEmail = appointment.PatientEmail,
                PatientImage = appointment.PatientImage,
                DoctorEmail = appointment.DoctorEmail,
                Date = appointment.Date,
                Time = appointment.Time,
                PatientSSN = appointment.PatientSSN,
                FirstName = appointment.Patientt.FirstName,
                LastName = appointment.Patientt.LastName,
                BirthDate = appointment.Patientt.BirthDate,
                Gender = appointment.Patientt.Gender,
                PhoneNumber = appointment.Patientt.PhoneNumber,
                DoctorId = appointment.DoctorId,
                Location = appointment.Doctor.Location,
                Price = appointment.Doctor.Price,
                Name = appointment.Doctor.Name,
            };
            return View(app);
        }

        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetAppointment(int id)
        {
            var appointment = await _unitOfWork.appointments.GetAppointment(id);
            if (appointment == null)
                return View("NotFound");

            var app = new AppointmentVM
            {
                Id = id,
                PateintName = appointment.PateintName,
                PatientEmail = appointment.PatientEmail,
                PatientImage = appointment.PatientImage,
                DoctorEmail = appointment.DoctorEmail,
                Date = appointment.date,
                Time = appointment.Time,
                PatientSSN = appointment.PatientSSN,
                FirstName = appointment.Patientt.FirstName,
                LastName = appointment.Patientt.LastName,
                BirthDate = appointment.Patientt.BirthDate,
                Gender = appointment.Patientt.Gender,
                PhoneNumber = appointment.Patientt.PhoneNumber,
                DoctorId = appointment.DoctorId,
                Location = appointment.Doctor.Location,
                Price = appointment.Doctor.Price,
                Name = appointment.Doctor.Name,
            };
            return View(app);
        }

        [Authorize(Roles = "User")]
        //Create Appointment
        public async Task<IActionResult> Create(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetDoctor(id);
            if (doctor == null)
                return View("NotFound");
            var DoctorDetail = new BookAppointmentViewModel
            {
                FirstName = doctor.User.FirstName,
                LastName = doctor.User.LastName,
                BirthDate = doctor.User.BirthDate,
                Email = doctor.User.Email,
                Gender = doctor.User.Gender,
                PhoneNumber = doctor.User.PhoneNumber,
                Name = doctor.Name,
                Location = doctor.Location,
                Zone = doctor.User.Zone,
                Price = doctor.Price,
                About = doctor.User.About,
                ProfileImg = doctor.User.ProfileImg,
            };
            return View(DoctorDetail);
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, BookAppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _toastNotification.AddErrorToastMessage("Please select time or date");
                return View("Error");
            }
            var doctor = await _unitOfWork.Doctors.GetDoctor(id);
            if (doctor == null)
                return View("NotFound");

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return NotFound("Register Or Login Please");
            var user = await _userManager.FindByIdAsync(userId);
            string patientEmail = User.FindFirstValue(ClaimTypes.Email);
            model.PatientID = userId;
            model.PatientEmail = patientEmail;

            var dateWithTime = _context.Appointments.Where(x => x.date == model.Date && x.Time == model.Time &&
            x.DoctorEmail == doctor.User.Email && x.PatientEmail == patientEmail).FirstOrDefault();
            if (dateWithTime != null)
            {
                _toastNotification.AddErrorToastMessage("This time with this date is booked before");
                return View("Error");
            }

            var appointment = new Appointment()
            {
                PatientID = model.PatientID,
                PateintName = $"{user.FirstName} {user.LastName}",
                PatientEmail = model.PatientEmail,
                PatientImage = user.ProfileImg,
                DoctorEmail = doctor.User.Email,
                date = model.Date,
                Time = model.Time,
                PhoneNumber = user.PhoneNumber,
                PatientSSN = (long)user.SSN,
                IsAccepted = false,
                DoctorId = doctor.Id,
            };

            await _unitOfWork.appointments.AddAsync(appointment);
            await _unitOfWork.Complete();
            _toastNotification.AddSuccessToastMessage("Appointment Created Successfully");
            return View("CompletedSuccessfully");
        }

        [Authorize(Roles = "User")]
        //Edit Appointment
        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _unitOfWork.appointments.GetAppointment(id);
            if (appointment == null)
                return View("NotFound");
            var appointmentVM = new EditAppointmentViewModel
            {
                PateintName = appointment.PateintName,
                Date = appointment.date,
                Time = appointment.Time,
                PatientPhone = appointment.PhoneNumber,
                //DoctorId = appointment.Id,
            };
            return View(appointmentVM);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditAppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _toastNotification.AddErrorToastMessage("Please select time or date");
                return View("Error");
            }

            var appointment = await _unitOfWork.appointments.GetAppointment(id);
            if (appointment == null)
                return View("NotFound");

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return NotFound("Register Or Login Please");
            var user = await _userManager.FindByIdAsync(userId);
            string patientEmail = User.FindFirstValue(ClaimTypes.Email);
            long patientSSN = (long)user.SSN;

            var dateWithTime = _context.Appointments.Where(x => x.date == model.Date && x.Time == model.Time &&
            x.DoctorEmail == appointment.DoctorEmail && x.PatientEmail == patientEmail).FirstOrDefault();
            if (dateWithTime != null)
            {
                _toastNotification.AddErrorToastMessage("This time with this date is booked before");
                return View("Error");
            }

            appointment.PatientID = userId;
            appointment.date = model.Date;
            appointment.Time = model.Time;
            appointment.PatientSSN = patientSSN;
            appointment.PateintName = model.PateintName;
            appointment.PatientImage = model.PatientImage;
            appointment.PatientEmail = patientEmail;
            appointment.PhoneNumber = user.PhoneNumber;
            appointment.IsAccepted = false;
            //appointment.DoctorId = model.DoctorId;

            await _unitOfWork.Complete();
            _toastNotification.AddSuccessToastMessage("Appointment Updated Successfully");
            return View("CompletedSuccessfully");
        }

        //Accept Appointment
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> AcceptsAppointment(int id, MessageViewModel model)
        {
            var appointment = await _unitOfWork.appointments.GetAppointment(id);
            if (appointment == null)
                return View("NotFound");
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string doctorEmail = User.FindFirstValue(ClaimTypes.Email);
            model.DoctorId = userId;
            model.DoctorEmail = doctorEmail;

            appointment.PatientEmail = appointment.PatientEmail;
            appointment.date = appointment.date;
            appointment.Time = appointment.Time;
            appointment.PhoneNumber = appointment.PhoneNumber;
            appointment.DoctorEmail = appointment.DoctorEmail;
            appointment.DoctorId = appointment.DoctorId;
            appointment.PatientSSN = appointment.PatientSSN;
            appointment.PatientImage = appointment.PatientImage;
            appointment.PateintName = appointment.PateintName;
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
                Messages = $"From Doctor: ({appointment.Doctor.Name}). " +
                $"Your Appointment with date ({appointment.date.ToString("dd MMMM yyyy")}) and time ({appointment.Time}) is Accepted",
                Date = DateTime.Now,
                PatientEmail = appointment.PatientEmail,
                DoctorEmail = doctorEmail,
                DoctorId = userId,
            };
            await _unitOfWork.appointments.AddMessageAsync(message);
            await _unitOfWork.Complete();
            _toastNotification.AddSuccessToastMessage($"Message has sent successfully");
            return RedirectToAction("GetAppointmentByEmail");
        }

        //Cancel Appointment by doctor
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CanceledAppointment(int id, MessageViewModel model)
        {
            var appointment = _unitOfWork.appointments.Get_Appointment(id);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string doctorEmail = User.FindFirstValue(ClaimTypes.Email);
            model.DoctorId = userId;
            model.DoctorEmail = doctorEmail;
            var cancelAppointment = new AcceptAndCancelAppointment
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
                IsAccepted = false,
                PatientID = appointment.PatientID,
            };
            await _unitOfWork.appointments.AddAcceptOrCancelAsync(cancelAppointment);
            await _unitOfWork.Complete();
            var message = new Message
            {
                Messages = $"From Doctor: ({appointment.Doctor.Name}). " +
                $"Sorry,Your Appointment with date ({appointment.date.ToString("dd MMMM yyyy")}) and time ({appointment.Time}) is Canceled because Doctor is busy in this time",
                Date = model.Date,
                PatientEmail = appointment.PatientEmail,
                DoctorEmail = doctorEmail,
                DoctorId = userId,
            };
            await _unitOfWork.appointments.AddMessageAsync(message);
            await _unitOfWork.Complete();
            var isDeleted = _unitOfWork.appointments.Canceled(id);
            return isDeleted ? Ok() : BadRequest();
        }

        //Cancel appointment by user
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Canceled(int id)
        {
            var appointment = _unitOfWork.appointments.Get_Appointment(id);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
            var isDeleted = _unitOfWork.appointments.Canceled(id);
            return isDeleted ? Ok() : BadRequest();
        }
    }
}
