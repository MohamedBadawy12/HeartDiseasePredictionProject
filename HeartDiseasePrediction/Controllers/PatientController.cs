using Database.Entities;
using HeartDiseasePrediction.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Repositories;
using Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace HeartDiseasePrediction.Controllers
{
	public class PatientController : Controller
	{
		private readonly IToastNotification _toastNotification;
		private readonly IUnitOfWork _unitOfWork;
		private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IFileRepository _fileRepository;
		public PatientController(IToastNotification toastNotification, AppDbContext context,
			IUnitOfWork unitOfWork, IFileRepository fileRepository)
		{
			_context = context;
			_unitOfWork = unitOfWork;
			_fileRepository = fileRepository;
			_toastNotification = toastNotification;
		}
		//get all Patients in list
		public async Task<IActionResult> Index()
		{
			var patients = await _unitOfWork.Patients.GetPatients();
			return View(patients);
		}
		[HttpPost]
		public async Task<IActionResult> Index(string search)
		{
			var patients = await _unitOfWork.Patients.FilterPatients(search);
			return View(patients);
		}

		//public async Task<ActionResult> Details(int id)
		//{
		//	var viewModel = new PatientDetailViewModel()
		//	{
		//		Patient = await _unitOfWork.Patients.GetPatient(id),
		//		Appointments = await _unitOfWork.appointments.GetAppointmentWithPatient(id),
		//		Attendances = await _unitOfWork.attendances.GetAttendance(id),
		//		CountAppointments = await _unitOfWork.appointments.CountAppointments(id),
		//		CountAttendance = await _unitOfWork.attendances.CountAttendances(id)
		//	};
		//	return View("Details", viewModel);
		//}

		//get Patient details
		public async Task<IActionResult> PatientDetails(long ssn)
		{
			try
			{
				var patient = await _unitOfWork.Patients.GetPatient(ssn);
				if (patient == null)
					return View("NotFound");

				var patientVM = new PatientVM
				{
					SSN = patient.SSN,
					Insurance_No = patient.Insurance_No,
					FirstName = patient.User.FirstName,
					LastName = patient.User.LastName,
					BirthDate = patient.User.BirthDate,
					Email = patient.User.Email,
					Gender = patient.User.Gender,
					PhoneNumber = patient.User.PhoneNumber,
					ProfileImg = patient.User.ProfileImg,
				};
				return View(patientVM);

			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
				return View();
			}
		}

		//Edit Patient
		[HttpGet]
		public async Task<IActionResult> Edit(long ssn)
		{
			try
			{
				var patient = await _unitOfWork.Patients.GetPatient(ssn);
				if (patient == null)
					return View("NotFound");

				var patientVM = new PatientVM
				{
					FirstName = patient.User.FirstName,
					LastName = patient.User.LastName,
					BirthDate = patient.User.BirthDate,
					Insurance_No = patient.Insurance_No,
					SSN = patient.SSN,
					//Insurance_No = patient.User.Insurance_No,
					Email = patient.User.Email,
					Gender = patient.User.Gender,
					PhoneNumber = patient.User.PhoneNumber,
					ProfileImg = patient.User.ProfileImg,
				};
				return View(patientVM);

			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
				return View();
			}
		}
		[HttpPost]
		public async Task<IActionResult> Edit(long ssn, PatientVM model)
		{
			try
			{
				var patient = await _unitOfWork.Patients.GetPatient(ssn);
				if (patient == null)
					return View("NotFound");

				//string wwwRootPath = _webHostEnvironment.WebRootPath;
				//string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
				//string extension = Path.GetExtension(model.ImageFile.FileName);
				//model.ProfileImg = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
				//string path = Path.Combine(wwwRootPath + "/Upload", fileName);

				//using (var fileStream = new FileStream(path, FileMode.Create))
				//{
				//    await model.ImageFile.CopyToAsync(fileStream);
				//}
				patient.User.PhoneNumber = model.PhoneNumber;
				patient.User.Email = model.Email;
				patient.User.FirstName = model.FirstName;
				patient.User.LastName = model.LastName;
				patient.User.BirthDate = model.BirthDate;
				patient.User.Gender = model.Gender;
				patient.Insurance_No = model.Insurance_No;
				patient.User.Insurance_No = model.Insurance_No;
				patient.User.SSN = model.SSN;
				patient.SSN = model.SSN;
				//doctor.User.ProfileImg = model.ProfileImg;

				_context.Patients.Update(patient);
				await _unitOfWork.Complete();
				_toastNotification.AddSuccessToastMessage("Patient Updated successfully");
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
				_toastNotification.AddErrorToastMessage("Patient Updated Failed");
				return View();
			}
		}

		//Delete Patient 
		public async Task<IActionResult> DeletePatient(long ssn)
		{
			var patient = _unitOfWork.Patients.Get_Patient(ssn);
			if (patient == null)
				return View("NotFound");
			//var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "/Upload", patient.User.ProfileImg);
			try
			{
				//if (System.IO.File.Exists(imagePath))
				//    System.IO.File.Delete(imagePath);
				//_patientService.Remove(patient);
				_unitOfWork.Patients.Remove(patient);
				await _unitOfWork.Complete();
				_toastNotification.AddSuccessToastMessage($"Patient with SSN {ssn} removed successfully");
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
				return View();
			}
		}

		//Delete Patient
		public IActionResult Delete(long ssn)
		{
			var isDeleted = _unitOfWork.Patients.Delete(ssn);
			return isDeleted ? Ok() : BadRequest();
		}
	}
}
