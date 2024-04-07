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
	public class ReciptionistController : Controller
	{
		private readonly IToastNotification _toastNotification;
		private readonly IUnitOfWork _unitOfWork;
		private readonly AppDbContext _context;
		private readonly IFileRepository _fileRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ReciptionistController(IToastNotification toastNotification
			, AppDbContext context, IUnitOfWork unitOfWork, IFileRepository fileRepository)
		{
			_toastNotification = toastNotification;
			_unitOfWork = unitOfWork;
			_fileRepository = fileRepository;
			_context = context;
		}
		//get all Reciptionists in list
		public async Task<IActionResult> Index()
		{
			var reciptionsits = await _unitOfWork.reciptionists.GetReciptionists();
			return View(reciptionsits);
		}
		[HttpPost]
		public async Task<IActionResult> Index(string search)
		{
			var reciptionsits = await _unitOfWork.reciptionists.FilterReciptionist(search);
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

				//string wwwRootPath = _webHostEnvironment.WebRootPath;
				//string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
				//string extension = Path.GetExtension(model.ImageFile.FileName);
				//model.ProfileImg = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
				//string path = Path.Combine(wwwRootPath + "/Upload", fileName);

				//using (var fileStream = new FileStream(path, FileMode.Create))
				//{
				//    await model.ImageFile.CopyToAsync(fileStream);
				//}
				reciptionsit.User.PhoneNumber = model.PhoneNumber;
				reciptionsit.User.Email = model.Email;
				reciptionsit.User.FirstName = model.FirstName;
				reciptionsit.User.LastName = model.LastName;
				reciptionsit.User.BirthDate = model.BirthDate;
				reciptionsit.User.Gender = model.Gender;
				//reciptionsit.User.ProfileImg = model.ProfileImg;

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
			//var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Upload", reciptionsit.User.ProfileImg);
			try
			{
				//if (System.IO.File.Exists(imagePath))
				//    System.IO.File.Delete(imagePath);
				//_reciptionistService.Remove(reciptionsit);
				_unitOfWork.reciptionists.Remove(reciptionsit);
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
