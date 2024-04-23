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
    public class LabAppointmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public LabAppointmentController(IUnitOfWork unitOfWork, IToastNotification toastNotification,
            IFileService fileRepository, AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;
            _context = context;
            _userManager = userManager;
        }

        //Get All Lab Appointments by User ID
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.labAppointment.GetLabAppointmentByUserId(userId, userRole);
            int totalRecords = appointments.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            appointments = appointments.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(appointments);
        }

        //Get All Appointments by Email
        [Authorize(Roles = "MedicalAnalyst")]
        public async Task<IActionResult> GetLabAppointmentByEmail(int currentPage = 1)
        {
            string labEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.labAppointment.GetWaitingLabAppointmentByEmail(labEmail, userRole);
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

        //Get All Accept and Cancel Appointments By Patient
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAcceptAndCancelLabAppointments(int currentPage = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.labAppointment.GetAcceptAndCancelLabAppointment(userId, userRole);
            int totalRecords = appointments.Count();
            int pageSize = 5;
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
        public async Task<IActionResult> GetAcceptAndCancelLabAppointments(DateTime? date, int currentPage = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.labAppointment.SearchAcceptAndCancelLabAppointment(userId, userRole, date);
            int totalRecords = appointments.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            appointments = appointments.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(appointments);
        }


        //Get All Accept Appointments By MedicalAnalyst
        [Authorize(Roles = "MedicalAnalyst")]
        public async Task<IActionResult> GetAcceptAppointments(int currentPage = 1)
        {
            string labEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.labAppointment.GetAcceptLabAppointmentByMedical(labEmail, userRole);
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
        [Authorize(Roles = "MedicalAnalyst")]
        [HttpPost]
        public async Task<IActionResult> GetAcceptAppointments(DateTime? date, long? ssn, int currentPage = 1)
        {
            string labEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var appointments = await _unitOfWork.labAppointment.SearchAcceptLabAppointments(labEmail, userRole, date, ssn);
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

        [Authorize(Roles = "MedicalAnalyst")]
        public async Task<IActionResult> GetAcceptAppointment(int id)
        {
            var appointment = await _unitOfWork.labAppointment.GetAcceptAppointment(id);
            if (appointment == null)
                return View("NotFound");

            var app = new LabAppointmentViewModel
            {
                Id = id,
                PateintName = appointment.PateintName,
                PatientEmail = appointment.PatientEmail,
                PatientImage = appointment.PatientImage,
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
            return View(app);
        }

        [Authorize(Roles = "MedicalAnalyst")]
        public async Task<IActionResult> GetAppointment(int id)
        {
            var appointment = await _unitOfWork.labAppointment.GetAppointment(id);
            if (appointment == null)
                return View("NotFound");

            var app = new LabAppointmentViewModel
            {
                Id = id,
                PateintName = appointment.PateintName,
                PatientEmail = appointment.PatientEmail,
                PatientImage = appointment.PatientImage,
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
            return View(app);
        }


        [Authorize(Roles = "User")]
        //Create Lab Appointment
        public async Task<IActionResult> Create(int id)
        {
            var lab = await _unitOfWork.labs.GetLab(id);
            if (lab == null)
                return View("NotFound");
            var labDetails = new BookLabAppointmentViewModel
            {
                LabPhoneNumber = lab.PhoneNumber,
                LabEmail = lab.User.Email,
                Name = lab.User.Name,
                Location = lab.User.Location,
                Price = lab.User.Price,
                Zone = lab.User.Zone,
                StartTime = lab.User.StartTime,
                EndTime = lab.User.EndTime,
                About = lab.User.About,
                LabImage = lab.LabImage,
            };
            return View(labDetails);
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, BookLabAppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _toastNotification.AddErrorToastMessage("Please select time or date");
                return View("Error");
            }
            var lab = await _unitOfWork.labs.GetLab(id);
            if (lab == null)
                return View("NotFound");

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return NotFound("Register Or Login Please");
            var user = await _userManager.FindByIdAsync(userId);
            string patientEmail = User.FindFirstValue(ClaimTypes.Email);
            model.PatientID = userId;
            model.PatientEmail = patientEmail;

            var dateWithTime = _context.LabAppointments.Where(x => x.date == model.Date && x.Time == model.Time &&
             x.LabEmail == lab.User.Email && x.PatientEmail == patientEmail).FirstOrDefault();
            if (dateWithTime != null)
            {
                _toastNotification.AddErrorToastMessage("This time with date is booked before");
                return View("Error");
            }


            var appointment = new LabAppointment()
            {
                PatientID = model.PatientID,
                PateintName = $"{user.FirstName} {user.LastName}",
                PatientEmail = model.PatientEmail,
                PatientImage = user.ProfileImg,
                LabEmail = lab.User.Email,
                date = model.Date,
                Time = model.Time,
                PhoneNumber = user.PhoneNumber,
                PatientSSN = (long)user.SSN,
                IsAccepted = false,
                LabId = lab.Id,
            };

            await _unitOfWork.labAppointment.AddAsync(appointment);
            await _unitOfWork.Complete();
            _toastNotification.AddSuccessToastMessage("Appointment Created Successfully");
            return View("CompletedSuccessfully");
        }

        [Authorize(Roles = "User")]
        //Create Appointment
        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _unitOfWork.labAppointment.GetAppointment(id);
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
            var appointment = await _unitOfWork.labAppointment.GetAppointment(id);
            if (appointment == null)
                return View("NotFound");

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return NotFound("Register Or Login Please");
            var user = await _userManager.FindByIdAsync(userId);
            string patientEmail = User.FindFirstValue(ClaimTypes.Email);
            long patientSSN = (long)user.SSN;

            var dateWithTime = _context.LabAppointments.Where(x => x.date == model.Date && x.Time == model.Time &&
             x.LabEmail == appointment.LabEmail && x.PatientEmail == patientEmail).FirstOrDefault();
            if (dateWithTime != null)
            {
                _toastNotification.AddErrorToastMessage("This time with date is booked before");
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
        [Authorize(Roles = "MedicalAnalyst")]
        public async Task<IActionResult> AcceptsLabAppointment(int id, MessageViewModel model)
        {
            var appointment = await _unitOfWork.labAppointment.GetAppointment(id);
            if (appointment == null)
                return View("NotFound");
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
                Messages = $"From Lab: ({appointment.Lab.Name}). " +
                $" Your Appointment with date ({appointment.date.ToString("dd MMMM yyyy")}) and time ({appointment.Time}) is Accepted",
                Date = DateTime.Now,
                PatientEmail = appointment.PatientEmail,
                DoctorEmail = labEmail,
                DoctorId = userId,
            };
            await _unitOfWork.appointments.AddMessageAsync(message);
            await _unitOfWork.Complete();
            _toastNotification.AddSuccessToastMessage($"Message has sent successfully");
            return RedirectToAction("GetLabAppointmentByEmail");
        }

        //Cancel Appointment by doctor
        [Authorize(Roles = "MedicalAnalyst")]
        public async Task<IActionResult> CanceledLabAppointment(int id, MessageViewModel model)
        {
            var appointment = _unitOfWork.labAppointment.Get_LabAppointment(id);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
                Messages = $"From Lab: ({appointment.Lab.Name}). " +
                $"Sorry,Your Appointment with date ({appointment.date.ToString("dd MMMM yyyy")}) and time ({appointment.Time}) is Canceled because Lab is busy in this time",
                Date = DateTime.Now,
                PatientEmail = appointment.PatientEmail,
                DoctorEmail = labEmail,
                DoctorId = userId,
            };
            await _unitOfWork.appointments.AddMessageAsync(message);
            await _unitOfWork.Complete();
            var isDeleted = _unitOfWork.labAppointment.Canceled(id);
            return isDeleted ? Ok() : BadRequest();
        }

        //Cancel appointment by user
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Canceled(int id)
        {
            var appointment = _unitOfWork.labAppointment.Get_LabAppointment(id);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
            var isDeleted = _unitOfWork.labAppointment.Canceled(id);
            return isDeleted ? Ok() : BadRequest();
        }
    }
}
