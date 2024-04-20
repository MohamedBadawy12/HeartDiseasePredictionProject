using Database.Entities;
using HeartDiseasePrediction.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Repositories;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HeartDiseasePrediction.Controllers
{
	public class MedicalTestsController : Controller
	{
		private readonly IToastNotification _toastNotification;
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly AppDbContext _context;
		public MedicalTestsController(IToastNotification toastNotification,
			AppDbContext context, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
		{
			_toastNotification = toastNotification;
			_unitOfWork = unitOfWork;
			_context = context;
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

		[AllowAnonymous]
		public async Task<IActionResult> MedicalTestDetails(int id)
		{
			try
			{
				var medicalTest = await _unitOfWork.medicalTest.GetMedicalTest(id);
				if (medicalTest == null)
					return View("NotFound");

				var medicalTestView = new PredictionViewModel
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
				var appointment = await _unitOfWork.labAppointment.GetAppointment(id);
				if (appointment == null)
					return View("NotFound");
				string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				string labEmail = User.FindFirstValue(ClaimTypes.Email);
				var user = await _userManager.FindByIdAsync(userId);
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
					Id = id,
					PatientName = medicalTest.PatientName,
					PatientEmail = medicalTest.PatientEmail,
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
				var medicalTest = await _unitOfWork.medicalTest.GetMedicalTest(id);
				if (medicalTest == null)
					return View("NotFound");


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

				_context.MedicalTests.Update(medicalTest);
				await _unitOfWork.Complete();
				_toastNotification.AddSuccessToastMessage("Predicted successfully");
				return RedirectToAction("Index");
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
	}
}
