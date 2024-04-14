using Database.Entities;
using HeartDiseasePrediction.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Repositories;
using Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HeartDiseasePrediction.Controllers
{
	public class ReciptionistController : Controller
	{
		private readonly IToastNotification _toastNotification;
		private readonly IUnitOfWork _unitOfWork;
		private readonly AppDbContext _context;
		private readonly IFileService _fileRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ReciptionistController(IToastNotification toastNotification
			, AppDbContext context, IUnitOfWork unitOfWork, IFileService fileRepository)
		{
			_toastNotification = toastNotification;
			_unitOfWork = unitOfWork;
			_fileRepository = fileRepository;
			_context = context;
		}
		//get all Reciptionists in list
		public async Task<IActionResult> Index(int currentPage = 1)
		{
			var reciptionsits = await _unitOfWork.reciptionists.GetReciptionists();
			int totalRecords = reciptionsits.Count();
			int pageSize = 8;
			int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
			reciptionsits = reciptionsits.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
			ViewBag.CurrentPage = currentPage;
			ViewBag.TotalPages = totalPages;
			ViewBag.HasPrevious = currentPage > 1;
			ViewBag.HasNext = currentPage < totalPages;
			return View(reciptionsits);
		}
		[HttpPost]
		public async Task<IActionResult> Index(string search, int currentPage = 1)
		{
			var reciptionsits = await _unitOfWork.reciptionists.FilterReciptionist(search);
			int totalRecords = reciptionsits.Count();
			int pageSize = 8;
			int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
			reciptionsits = reciptionsits.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
			ViewBag.CurrentPage = currentPage;
			ViewBag.TotalPages = totalPages;
			ViewBag.HasPrevious = currentPage > 1;
			ViewBag.HasNext = currentPage < totalPages;
			return View(reciptionsits);
		}
		//get Reciptionist details
		public async Task<IActionResult> ReciptionistDetails(int id)
		{
			try
			{
				var reciptionsit = await _unitOfWork.reciptionists.GetReciptionist(id);
				if (reciptionsit == null)
					return View("NotFound");

				var reciptionistVM = new ReciptionistVM
				{
					FirstName = reciptionsit.User.FirstName,
					LastName = reciptionsit.User.LastName,
					BirthDate = reciptionsit.User.BirthDate,
					Email = reciptionsit.User.Email,
					Gender = reciptionsit.User.Gender,
					PhoneNumber = reciptionsit.User.PhoneNumber,
					ProfileImg = reciptionsit.User.ProfileImg,
				};
				return View(reciptionistVM);
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
				return View();
			}
		}

		//Edit Reciptionist
		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			try
			{
				var reciptionsit = await _unitOfWork.reciptionists.GetReciptionist(id);
				if (reciptionsit == null)
					return View("NotFound");

				var reciptionistVM = new ReciptionistVM
				{
					FirstName = reciptionsit.User.FirstName,
					LastName = reciptionsit.User.LastName,
					BirthDate = reciptionsit.User.BirthDate,
					Email = reciptionsit.User.Email,
					Gender = reciptionsit.User.Gender,
					PhoneNumber = reciptionsit.User.PhoneNumber,
					ProfileImg = reciptionsit.User.ProfileImg,
				};
				return View(reciptionistVM);
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
				return View();
			}
		}
		[HttpPost]
		public async Task<IActionResult> Edit(int id, ReciptionistVM model)
		{
			try
			{
				var reciptionsit = await _unitOfWork.reciptionists.GetReciptionist(id);
				if (reciptionsit == null)
					return View("NotFound");

				var path = model.ProfileImg;
				if (model.ImageFile?.Length > 0)
				{
					_fileRepository.DeleteImage(path);
					path = await _fileRepository.UploadAsync(model.ImageFile, "/Uploads/");
					if (path == "An Problem occured when creating file")
					{
						return BadRequest();
					}
				}
				model.ProfileImg = path;

				reciptionsit.User.PhoneNumber = model.PhoneNumber;
				reciptionsit.User.Email = model.Email;
				reciptionsit.User.FirstName = model.FirstName;
				reciptionsit.User.LastName = model.LastName;
				reciptionsit.User.BirthDate = model.BirthDate;
				reciptionsit.User.Gender = model.Gender;
				reciptionsit.User.ProfileImg = model.ProfileImg;

				_context.Reciptionists.Update(reciptionsit);
				await _unitOfWork.Complete();
				_toastNotification.AddSuccessToastMessage("Reciptionsit Updated successfully");
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
				_toastNotification.AddErrorToastMessage("Reciptionsit Updated Failed");
				return View();
			}
		}

		//Delete Reciptionist 
		public async Task<IActionResult> DeleteReciptionist(int id)
		{
			var reciptionsit = await _unitOfWork.reciptionists.GetReciptionist(id);
			if (reciptionsit == null)
				return View("NotFound");
			try
			{
				string path = "";
				path = reciptionsit.User.ProfileImg;
				_unitOfWork.reciptionists.Remove(reciptionsit);
				_fileRepository.DeleteImage(path);
				await _unitOfWork.Complete();
				_toastNotification.AddSuccessToastMessage($"Reciptionsit with ID {id} removed successfully");
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
				return View();
			}
		}

		//Delete Reciptionist 
		public IActionResult Delete(int id)
		{
			var isDeleted = _unitOfWork.reciptionists.Delete(id);
			return isDeleted ? Ok() : BadRequest();
		}
	}
}
