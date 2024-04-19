using Database.Entities;
using HeartDiseasePrediction.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Repositories;
using Repositories.ViewModel;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HeartDiseasePrediction.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        public PrescriptionController(IToastNotification toastNotification,
            AppDbContext context, IUnitOfWork unitOfWork)
        {
            _toastNotification = toastNotification;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        //Get All Prescriptions By User ID
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var prescriptions = await _unitOfWork.prescriptions.GetPrescriptionByUserId(userId, userRole);
            int totalRecords = prescriptions.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            prescriptions = prescriptions.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(prescriptions);
        }
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> Index(DateTime? date, long? ssn, int currentPage = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var prescriptions = await _unitOfWork.prescriptions.SearchPrescriptionByUserId(userId, userRole, date, ssn);
            int totalRecords = prescriptions.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            prescriptions = prescriptions.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(prescriptions);
        }

        //Get All Prescriptions
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetPrescriptions(int currentPage = 1)
        {
            string PatientEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var prescriptions = await _unitOfWork.prescriptions.GetPrescriptionByEmail(PatientEmail, userRole);
            int totalRecords = prescriptions.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            prescriptions = prescriptions.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(prescriptions);
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> GetPrescriptions(DateTime? date, string? doctorName, int currentPage = 1)
        {
            string PatientEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var prescriptions = await _unitOfWork.prescriptions.SearchPrescriptionByEmail(PatientEmail, userRole, date, doctorName);
            int totalRecords = prescriptions.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            prescriptions = prescriptions.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(prescriptions);
        }

        //Get All Prescriptions patient with this doctor
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetPatientPrescriptions(int id, int currentPage = 1)
        {
            var appointment = await _unitOfWork.appointments.GetAcceptAppointment(id);
            if (appointment == null)
                return View("NotFound");
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string doctorEmail = User.FindFirstValue(ClaimTypes.Email);
            var prescriptions = await _context.Prescriptions.Where(x => x.PatientSSN == appointment.PatientSSN &&
            x.DoctorEmail == doctorEmail && x.ApDoctorId == userId).ToListAsync();
            int totalRecords = prescriptions.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            prescriptions = prescriptions.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(prescriptions);
        }
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> GetPatientPrescriptions(int id, DateTime? date, int currentPage = 1)
        {
            var appointment = await _unitOfWork.appointments.GetAcceptAppointment(id);
            if (appointment == null)
                return View("NotFound");
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string doctorEmail = User.FindFirstValue(ClaimTypes.Email);
            var prescriptionss = await _context.Prescriptions.Where(x => x.PatientSSN == appointment.PatientSSN &&
            x.DoctorEmail == doctorEmail && x.ApDoctorId == userId).ToListAsync();
            if ((date.HasValue && date != null) || date == DateTime.MinValue)
            {
                var prescriptions = await _context.Prescriptions.Where(x => date.HasValue && x.date.Year == date.Value.Year && x.date.Month == date.Value.Month && x.date.Day == date.Value.Day &&
                  x.PatientSSN == appointment.PatientSSN &&
                  x.DoctorEmail == doctorEmail && x.ApDoctorId == userId)
                  .Include(d => d.Doctor).Include(p => p.Patient).ToListAsync();
                int totalRecords = prescriptions.Count();
                int pageSize = 8;
                int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                prescriptions = prescriptions.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.CurrentPage = currentPage;
                ViewBag.TotalPages = totalPages;
                ViewBag.HasPrevious = currentPage > 1;
                ViewBag.HasNext = currentPage < totalPages;
                return View(prescriptions);
            }
            return View(prescriptionss);
        }

        //Search for Prescriptions
        //[HttpPost]
        //public async Task<IActionResult> Index(long search)
        //{
        //	var prescriptions = await _unitOfWork.prescriptions.FilterPrescriptions(search);
        //	return View(prescriptions);
        //}

        //get Prescription details
        [AllowAnonymous]
        public async Task<IActionResult> PrescriptionDetails(int id)
        {
            try
            {
                var prescription = await _unitOfWork.prescriptions.GetPrescription(id);
                if (prescription == null)
                    return View("NotFound");

                var prescriptionVM = new PrescriptionVM
                {
                    MedicineName = prescription.MedicineName,
                    PatientSSN = prescription.PatientSSN,
                    date = prescription.date,
                    ApDoctorId = prescription.ApDoctorId,
                    DoctorEmail = prescription.DoctorEmail,
                    PatientEmail = prescription.PatientEmail,
                    DoctorId = prescription.DoctorId,
                    DoctorName = prescription.Doctor.Name,
                };
                return View(prescriptionVM);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        //Create Prescriptions
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Create()
        {
            var doctorDropDownList = await _unitOfWork.prescriptions.GetDoctorDropDownsValues();
            ViewBag.Doctor = new SelectList(doctorDropDownList.doctors, "Id", "Name");
            return View();
        }
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> Create(PrescriptionViewModel model)
        {
            try
            {
                var doctorDropDownList = await _unitOfWork.prescriptions.GetDoctorDropDownsValues();
                ViewBag.Doctor = new SelectList(doctorDropDownList.doctors, "Id", "Name");
                var patient = await _unitOfWork.Patients.GetPatient(model.PatientSSN);
                if (patient == null)
                    return View("NotFound");
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string doctorEmail = User.FindFirstValue(ClaimTypes.Email);
                model.ApDoctorId = userId;
                model.DoctorEmail = doctorEmail;
                await _unitOfWork.prescriptions.AddPrescriptionAsync(model);
                _toastNotification.AddSuccessToastMessage("Prescription Created successfully");
                return View("CompletedSuccessfully");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                _toastNotification.AddErrorToastMessage("An error occurred while saving the prescription.");
                return View(model);
            }
        }

        //Create Prescriptions
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreatePresciptions()
        {
            return View();
        }
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> CreatePresciptions(int id, PrescriptionViewModel model)
        {
            try
            {
                var appointment = await _unitOfWork.appointments.GetAppointment(id);
                if (appointment == null)
                    return View("NotFound");
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string doctorEmail = User.FindFirstValue(ClaimTypes.Email);
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
                _toastNotification.AddSuccessToastMessage("Prescription Created successfully");
                return View("CompletedSuccessfully");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                _toastNotification.AddErrorToastMessage("An error occurred while saving the prescription.");
                return View(model);
            }
        }

        //Edit details of Prescription
        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var prescription = await _unitOfWork.prescriptions.GetPrescription(id);
                if (prescription == null)
                    return View("NotFound");
                var doctorDropDownList = await _unitOfWork.prescriptions.GetDoctorDropDownsValues();
                ViewBag.Doctor = new SelectList(doctorDropDownList.doctors, "Id", "Name");
                var prescriptionVM = new Prescription
                {
                    MedicineName = prescription.MedicineName,
                    PatientSSN = prescription.PatientSSN,
                    date = prescription.date,
                    ApDoctorId = prescription.ApDoctorId,
                    DoctorEmail = prescription.DoctorEmail,
                    PatientEmail = prescription.PatientEmail,
                    DoctorId = prescription.DoctorId,
                };
                return View(prescriptionVM);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Prescription model)
        {
            try
            {
                var prescription = await _unitOfWork.prescriptions.GetPrescription(id);
                if (prescription == null)
                    return View("NotFound");

                var doctorDropDownList = await _unitOfWork.prescriptions.GetDoctorDropDownsValues();
                ViewBag.Doctor = new SelectList(doctorDropDownList.doctors, "Id", "Name");
                prescription.MedicineName = model.MedicineName;
                prescription.PatientSSN = model.PatientSSN;
                prescription.date = model.date;
                prescription.DoctorId = model.DoctorId;
                prescription.ApDoctorId = model.ApDoctorId;
                prescription.DoctorEmail = model.DoctorEmail;
                prescription.PatientEmail = model.PatientEmail;
                //prescription.Doctor = model.Doctor;

                _context.Prescriptions.Update(prescription);
                await _unitOfWork.Complete();
                _toastNotification.AddSuccessToastMessage("Prescription Updated successfully");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                _toastNotification.AddErrorToastMessage("Prescription Updated Failed");
                return View();
            }
        }

        //Delete Prescription 
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var prescription = _unitOfWork.prescriptions.Get_Prescription(id);
            if (prescription != null)
            {
                //_prescriptionService.Remove(prescription);
                _unitOfWork.prescriptions.Remove(prescription);
                await _unitOfWork.Complete();
                _toastNotification.AddSuccessToastMessage($"Prescription with ID {id} removed successfully");
            }
            return RedirectToAction("Index");
        }

        //Delete Prescription 
        [Authorize(Roles = "Doctor")]
        public IActionResult Delete(int id)
        {
            var isDeleted = _unitOfWork.prescriptions.Delete(id);
            return isDeleted ? Ok() : BadRequest();
        }
    }
}
