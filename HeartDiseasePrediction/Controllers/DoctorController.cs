using Database.Entities;
using HeartDiseasePrediction.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Repositories;
using Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace HeartDiseasePrediction.Controllers
{
	[Authorize(Roles = "Admin")]
	public class DoctorController : Controller
	{
		private readonly IToastNotification _toastNotification;
		private readonly IUnitOfWork _unitOfWork;
		private readonly AppDbContext _context;
		private readonly IFileRepository _fileRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public DoctorController(IToastNotification toastNotification, AppDbContext context
			, IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork, IFileRepository fileRepository)
		{
			_toastNotification = toastNotification;
			_unitOfWork = unitOfWork;
			_context = context;
			_fileRepository = fileRepository;
			_webHostEnvironment = webHostEnvironment;
		}

		//get all doctors in list
		public async Task<IActionResult> Index()
		{
			var doctors = await _unitOfWork.Doctors.GetDoctors();
			return View(doctors);
		}
		//Search 
		[HttpPost]
		public async Task<IActionResult> Index(string search)
		{
			var doctorViewModel = await _unitOfWork.Doctors.FilterDoctors(search);
			return View(doctorViewModel);
		}
		[AllowAnonymous]
		public async Task<IActionResult> DoctorsList()
		{
			var doctors = await _unitOfWork.Doctors.GetDoctors();
			return View(doctors);
		}
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> DoctorsList(string search)
		{
			var doctors = await _unitOfWork.Doctors.FilterDoctors(search);
			return View(doctors);
		}
		//get doctor details
		public async Task<IActionResult> DoctorDetails(int id)
		{
			try
			{
				var doctor = await _unitOfWork.Doctors.GetDoctor(id);
				if (doctor == null)
					return View("NotFound");

				var DoctorDetail = new DoctorVM
				{
					FirstName = doctor.User.FirstName,
					LastName = doctor.User.LastName,
					BirthDate = doctor.User.BirthDate,
					Email = doctor.User.Email,
					Gender = doctor.User.Gender,
					PhoneNumber = doctor.User.PhoneNumber,
					ProfileImg = doctor.User.ProfileImg,
					Name = doctor.Name,
					Location = doctor.Location,
					Price = doctor.Price,
				};
				return View(DoctorDetail);
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
				return View();
			}
		}
		[HttpGet]
		public async Task<IActionResult> DoctorProfile()
		{
			return View();
		}

		//Edit Doctor
		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			try
			{
				var doctor = await _unitOfWork.Doctors.GetDoctor(id);
				if (doctor == null)
					return View("NotFound");

				var DoctorDetail = new DoctorVM
				{
					FirstName = doctor.User.FirstName,
					LastName = doctor.User.LastName,
					BirthDate = doctor.User.BirthDate,
					Email = doctor.User.Email,
					Gender = doctor.User.Gender,
					PhoneNumber = doctor.User.PhoneNumber,
					Name = doctor.Name,
					Location = doctor.Location,
					Price = doctor.Price,
					//ProfileImg = doctor.User.ProfileImg,
				};
				return View(DoctorDetail);
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, DoctorVM model)
		{
			try
			{
				var doctor = await _unitOfWork.Doctors.GetDoctor(id);
				if (doctor == null)
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
				doctor.User.PhoneNumber = model.PhoneNumber;
				doctor.User.Email = model.Email;
				doctor.User.FirstName = model.FirstName;
				doctor.User.LastName = model.LastName;
				doctor.User.BirthDate = model.BirthDate;
				doctor.User.Name = model.Name;
				doctor.User.Location = model.Location;
				doctor.User.Price = model.Price;
				doctor.User.Gender = model.Gender;
				doctor.Name = model.Name;
				doctor.Location = model.Location;
				doctor.Price = model.Price;
				//doctor.User.ProfileImg = model.ProfileImg;

				_context.Doctors.Update(doctor);
				await _unitOfWork.Complete();
				_toastNotification.AddSuccessToastMessage("Doctor Updated successfully");
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
				_toastNotification.AddErrorToastMessage("Doctor Updated Failed");
				return View();
			}
		}
		//Delete docotor 
		public async Task<IActionResult> DeleteDoctor(int id)
		{
			var doctor = _unitOfWork.Doctors.Get_Doctor(id);
			if (doctor == null)
				return View("NotFound");
			//var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Upload", doctor.User.ProfileImg);
			try
			{
				//if (System.IO.File.Exists(imagePath))
				//    System.IO.File.Delete(imagePath);
				//_doctorService.Delete(doctor);
				_unitOfWork.Doctors.Delete(doctor);
				await _unitOfWork.Complete();
				_toastNotification.AddSuccessToastMessage($"Doctor with ID {id} removed successfully");
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
				return View();
			}
		}

		//Delete Doctor
		public IActionResult Delete(int id)
		{
			var isDeleted = _unitOfWork.Doctors.DeleteDoctor(id);
			return isDeleted ? Ok() : BadRequest();
		}
	}
}
