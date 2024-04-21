using Database.Entities;
using HeartDiseasePrediction.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using Repositories;
using Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HeartDiseasePrediction.Controllers
{
    public class MedicalAnalystController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileRepository;
        public MedicalAnalystController(IToastNotification toastNotification, AppDbContext context,
            IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork, IFileService fileRepository)
        {
            _toastNotification = toastNotification;
            _context = context;
            _unitOfWork = unitOfWork;
            _fileRepository = fileRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        //get all Medical Analysts in list
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var medicalAnalyst = await _unitOfWork.medicalAnalysts.GetMedicalAnalysts();
            int totalRecords = medicalAnalyst.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            medicalAnalyst = medicalAnalyst.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(medicalAnalyst);
        }
        //search
        [HttpPost]
        public async Task<IActionResult> Index(string search, int currentPage = 1)
        {
            var medicalAnalyst = await _unitOfWork.medicalAnalysts.FilterMedicalAnalyst(search);
            int totalRecords = medicalAnalyst.Count();
            int pageSize = 8;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            medicalAnalyst = medicalAnalyst.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(medicalAnalyst);
        }
        //Medical Analyst Details
        public async Task<IActionResult> MedicalAnalystDetails(int id)
        {
            try
            {
                var medicalAnalyst = await _unitOfWork.medicalAnalysts.GetMedicalAnalyst(id);
                if (medicalAnalyst == null)
                    return View("NotFound");

                var medicalAnalystVM = new MedicalAnalystVM
                {
                    FirstName = medicalAnalyst.User.FirstName,
                    LastName = medicalAnalyst.User.LastName,
                    BirthDate = medicalAnalyst.User.BirthDate,
                    Email = medicalAnalyst.User.Email,
                    Gender = medicalAnalyst.User.Gender,
                    PhoneNumber = medicalAnalyst.User.PhoneNumber,
                    ProfileImg = medicalAnalyst.User.ProfileImg,
                    LabId = medicalAnalyst.LabId,
                    LabName = medicalAnalyst.Lab.Name,
                };
                return View(medicalAnalystVM);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        //Edit details of Medical Analyst
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var medicalAnalyst = await _unitOfWork.medicalAnalysts.GetMedicalAnalyst(id);
                if (medicalAnalyst == null)
                    return View("NotFound");

                var medicalAnalystVM = new MedicalAnalystVM
                {
                    FirstName = medicalAnalyst.User.FirstName,
                    LastName = medicalAnalyst.User.LastName,
                    BirthDate = medicalAnalyst.User.BirthDate,
                    Email = medicalAnalyst.User.Email,
                    Gender = medicalAnalyst.User.Gender,
                    PhoneNumber = medicalAnalyst.User.PhoneNumber,
                    ProfileImg = medicalAnalyst.User.ProfileImg,
                    LabId = medicalAnalyst.LabId,
                };
                var labDropDownList = await _unitOfWork.labs.GetLabDropDownsValues();
                ViewBag.Lab = new SelectList(labDropDownList.labs, "Id", "Name");
                return View(medicalAnalystVM);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, MedicalAnalystVM model)
        {
            try
            {
                var medicalAnalyst = await _unitOfWork.medicalAnalysts.GetMedicalAnalyst(id);
                if (medicalAnalyst == null)
                    return View("NotFound");

                if (!ModelState.IsValid)
                {
                    var labDropDownList = await _unitOfWork.labs.GetLabDropDownsValues();
                    ViewBag.Lab = new SelectList(labDropDownList.labs, "Id", "Name");
                    return View(model);
                }
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

                medicalAnalyst.User.PhoneNumber = model.PhoneNumber;
                medicalAnalyst.User.Email = model.Email;
                medicalAnalyst.User.FirstName = model.FirstName;
                medicalAnalyst.User.LastName = model.LastName;
                medicalAnalyst.User.BirthDate = model.BirthDate;
                medicalAnalyst.User.Gender = model.Gender;
                medicalAnalyst.LabId = model.LabId;
                medicalAnalyst.User.ProfileImg = model.ProfileImg;

                _context.MedicalAnalysts.Update(medicalAnalyst);
                await _unitOfWork.Complete();
                _toastNotification.AddSuccessToastMessage("MedicalAnalyst Updated successfully");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                _toastNotification.AddErrorToastMessage("MedicalAnalyst Updated Failed");
                return View();
            }
        }

        //Delete Medical Analyst 
        public async Task<IActionResult> DeleteMedicalAnalyst(int id)
        {
            var medicalAnalyst = _unitOfWork.medicalAnalysts.Get_MedicalAnalyst(id);
            if (medicalAnalyst == null)
                return View("NotFound");

            try
            {
                string path = "";
                path = medicalAnalyst.User.ProfileImg;
                _unitOfWork.medicalAnalysts.Remove(medicalAnalyst);
                _fileRepository.DeleteImage(path);
                await _unitOfWork.Complete();
                _toastNotification.AddSuccessToastMessage($"MedicalAnalyst with ID {id} removed successfully");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        //Delete Medical Analyst
        public IActionResult Delete(int id)
        {
            var isDeleted = _unitOfWork.medicalAnalysts.Delete(id);
            return isDeleted ? Ok() : BadRequest();
        }
    }
}
