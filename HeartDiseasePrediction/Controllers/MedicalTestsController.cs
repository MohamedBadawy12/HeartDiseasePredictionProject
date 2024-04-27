using Database.Entities;
using HeartDiseasePrediction.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NToastNotify;
using Repositories;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HeartDiseasePrediction.Controllers
{
    public class MedicalTestsController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly IUnitOfWork _unitOfWork;
        //Uri baseAddress = new Uri("https://heart-project-2-2.onrender.com/predict");
        HttpClient _client;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public MedicalTestsController(IToastNotification toastNotification,
            AppDbContext context, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _toastNotification = toastNotification;
            _unitOfWork = unitOfWork;
            _context = context;
            _client = new HttpClient();
            _userManager = userManager;
        }
        //Get MedicalTests by userId
        [Authorize(Roles = "MedicalAnalyst")]
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var medicalTests = await _unitOfWork.medicalTest.GetMedicalTestsByUserId(userId, userRole);
            int totalRecords = medicalTests.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            medicalTests = medicalTests.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(medicalTests);
        }

        //Search For Medical Test
        [Authorize(Roles = "MedicalAnalyst")]
        [HttpPost]
        public async Task<IActionResult> Index(DateTime? date, long? ssn, int currentPage = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var medicalTests = await _unitOfWork.medicalTest.SearchMedicalTestsByUserId(userId, userRole, date, ssn);
            int totalRecords = medicalTests.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            medicalTests = medicalTests.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(medicalTests);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetMedicalTests(int currentPage = 1)
        {
            string PatientEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var medicalTests = await _unitOfWork.medicalTest.GetMedicalTestsByEmail(PatientEmail, userRole);
            int totalRecords = medicalTests.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            medicalTests = medicalTests.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(medicalTests);
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> GetMedicalTests(DateTime? date, int currentPage = 1)
        {
            string PatientEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var medicalTests = await _unitOfWork.medicalTest.SearchMedicalTestsByEmail(PatientEmail, userRole, date);
            int totalRecords = medicalTests.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            medicalTests = medicalTests.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(medicalTests);
        }

        [Authorize(Roles = "MedicalAnalyst")]
        public async Task<IActionResult> GetPatientMedicalTests(int id, int currentPage = 1)
        {
            var appointment = await _unitOfWork.labAppointment.GetAcceptAppointment(id);
            if (appointment == null)
                return View("NotFound");
            string labEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var medicalTests = await _context.MedicalTests.Where(x => x.PatientEmail == appointment.PatientEmail
            && x.LabEmail == labEmail).Include(d => d.Patient).Include(p => p.Lab).ToListAsync();
            int totalRecords = medicalTests.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            medicalTests = medicalTests.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(medicalTests);
        }
        [Authorize(Roles = "MedicalAnalyst")]
        [HttpPost]
        public async Task<IActionResult> GetPatientMedicalTests(int id, DateTime? date, int currentPage = 1)
        {
            var appointment = await _unitOfWork.labAppointment.GetAcceptAppointment(id);
            if (appointment == null)
                return View("NotFound");
            string labEmail = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var medicalTests = await _context.MedicalTests.Where(x => x.PatientEmail == appointment.PatientEmail
            && x.LabEmail == labEmail).Include(d => d.Patient).Include(p => p.Lab).ToListAsync();
            if ((date.HasValue && date != null) || date == DateTime.MinValue)
            {
                var medicalTest = await _context.MedicalTests.Where(x => date.HasValue && x.Date.Year == date.Value.Year && x.Date.Month == date.Value.Month && x.Date.Day == date.Value.Day &&
                 x.PatientEmail == appointment.PatientEmail &&
                 x.LabEmail == labEmail)
                 .Include(d => d.Patient).Include(p => p.Lab).ToListAsync();
                int totalRecords = medicalTest.Count();
                int pageSize = 8;
                int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                medicalTest = medicalTest.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.CurrentPage = currentPage;
                ViewBag.TotalPages = totalPages;
                ViewBag.HasPrevious = currentPage > 1;
                ViewBag.HasNext = currentPage < totalPages;
                return View(medicalTest);
            }
            return View(medicalTests);
        }

        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetPatientTests(int id, int currentPage = 1)
        {
            var appointment = await _unitOfWork.appointments.GetAcceptAppointment(id);
            if (appointment == null)
                return View("NotFound");
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //string doctorEmail = User.FindFirstValue(ClaimTypes.Email);
            var medicalTests = await _context.MedicalTests.Where(x => x.PatientSSN == appointment.PatientSSN &&
            x.PatientEmail == appointment.PatientEmail).ToListAsync();
            int totalRecords = medicalTests.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            medicalTests = medicalTests.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(medicalTests);
        }
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> GetPatientTests(int id, DateTime? date, int currentPage = 1)
        {
            var appointment = await _unitOfWork.appointments.GetAcceptAppointment(id);
            if (appointment == null)
                return View("NotFound");
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //string doctorEmail = User.FindFirstValue(ClaimTypes.Email);
            var medicalTests = await _context.MedicalTests.Where(x => x.PatientSSN == appointment.PatientSSN &&
           x.PatientEmail == appointment.PatientEmail).ToListAsync();
            if ((date.HasValue && date != null) || date == DateTime.MinValue)
            {
                var medicalTestss = await _context.MedicalTests.Where(x => x.PatientSSN == appointment.PatientSSN &&
                    x.PatientEmail == appointment.PatientEmail).ToListAsync();
                int totalRecords = medicalTestss.Count();
                int pageSize = 8;
                int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                medicalTestss = medicalTestss.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.CurrentPage = currentPage;
                ViewBag.TotalPages = totalPages;
                ViewBag.HasPrevious = currentPage > 1;
                ViewBag.HasNext = currentPage < totalPages;
                return View(medicalTestss);
            }
            return View(medicalTests);
        }

        [AllowAnonymous]
        public async Task<IActionResult> MedicalTestDetails(int id)
        {
            try
            {
                var medicalTest = await _unitOfWork.medicalTest.GetMedicalTest(id);
                if (medicalTest == null)
                    return View("NotFound");

                var medicalTestView = new PredictionDetailsViewModel
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
                    HeartRate = medicalTest.HeartRate,
                    PrevalentStroke = medicalTest.PrevalentStroke,
                    Prediction = medicalTest.Prediction,
                    Probability = medicalTest.Probability,
                };
                return View(medicalTestView);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        //Create MedicalTest and make prediction for it
        [Authorize(Roles = "MedicalAnalyst")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [Authorize(Roles = "MedicalAnalyst")]
        [HttpPost]
        public async Task<IActionResult> Create(int id, MedicalTestViewModel model)
        {
            try
            {
                var appointment = await _unitOfWork.labAppointment.GetAcceptAppointment(id);
                if (appointment == null)
                    return View("NotFound");
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string labEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByIdAsync(userId);
                var medicalTest = new MedicalTest
                {
                    UserId = userId,
                    PatientName = $"{appointment.Patientt.FirstName} {appointment.Patientt.LastName}",
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
                    HeartRate = model.HeartRate,
                    PrevalentStroke = model.PrevalentStroke,
                };
                await _unitOfWork.medicalTest.AddAsync(medicalTest);
                await _unitOfWork.Complete();
                _toastNotification.AddSuccessToastMessage("Medical Test Created successfully");
                return View("CompletedSuccessfully");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                _toastNotification.AddErrorToastMessage("An error occurred while saving the Medical Test.");
                return View(model);
            }
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Prediction(int id)
        {
            try
            {
                var medicalTest = await _unitOfWork.medicalTest.GetMedicalTest(id);
                if (medicalTest == null)
                    return View("NotFound");

                var medicalTestView = new PredictionViewModel
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
                };

                return View(medicalTestView);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Prediction(int id, PredictionViewModel model)
        {

            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync("https://heart-project-2-4.onrender.com/predict", content);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject prediction = JObject.Parse(responseBody);
                    ViewBag.Prediction = prediction["prediction"].ToString();
                    ViewBag.Probability = prediction["Risk Rate"];
                    var medicalTest = await _unitOfWork.medicalTest.GetMedicalTest(id);
                    if (medicalTest == null)
                        return View("NotFound");

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
                    medicalTest.Probability = (float?)prediction["Risk Rate"];
                    if (prediction["prediction"].ToString() == "Has Heart Disease")
                    {
                        medicalTest.Prediction = 1;
                    }
                    else
                    {
                        medicalTest.Prediction = 0;
                    }

                    _context.MedicalTests.Update(medicalTest);
                    await _unitOfWork.Complete();
                    _toastNotification.AddSuccessToastMessage("Predicted successfully");
                    return View("PredictionResult");
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Predicted Failed");
                    return View();
                }

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                _toastNotification.AddErrorToastMessage("Predicted Failed");
                return View();
            }
        }

        //Delete Medical Test 
        [Authorize(Roles = "MedicalAnalyst")]
        public IActionResult Delete(int id)
        {
            var isDeleted = _unitOfWork.medicalTest.Delete(id);
            return isDeleted ? Ok() : BadRequest();
        }
        private string ParseResponse(string responseBody)
        {
            try
            {
                // Parse JSON response and extract prediction
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);

                // Assuming the response contains a "prediction" field
                string prediction = jsonResponse.prediction;

                return prediction;
            }
            catch (Exception ex)
            {
                // Handle parsing errors
                Console.WriteLine($"Error parsing JSON response: {ex.Message}");
                return null;
            }
        }

    }
}
