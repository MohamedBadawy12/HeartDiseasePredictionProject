using Database.Entities;
using HeartDiseasePrediction.ViewModel;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public class LabController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly IFileService _fileRepository;
        public LabController(IToastNotification toastNotification, AppDbContext context
            , IUnitOfWork unitOfWork, IFileService fileRepository)
        {
            _toastNotification = toastNotification;
            _unitOfWork = unitOfWork;
            _context = context;
            _fileRepository = fileRepository;
        }
        //Lab List
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var labs = await _unitOfWork.labs.GetLabs();
            int totalRecords = labs.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            labs = labs.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            ViewBag.msg = TempData["msg"] as string;
            return View(labs);
        }

        //Lab List
        [AllowAnonymous]
        public async Task<IActionResult> GetAllLabs(int currentPage = 1)
        {
            var labs = await _unitOfWork.labs.GetLabs();
            var ZoneDropDownList = await _unitOfWork.labs.GetLabZoneDropDownsValues();
            ViewBag.Zone = new SelectList(ZoneDropDownList.Zones, "Id", "Zone");
            int totalRecords = labs.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            labs = labs.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            ViewBag.msg = TempData["msg"] as string;
            return View(labs);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetAllLabs(string name, string zone, int currentPage = 1)
        {
            var labs = await _unitOfWork.labs.SearchForLab(name, zone);
            var ZoneDropDownList = await _unitOfWork.labs.GetLabZoneDropDownsValues();
            ViewBag.Zone = new SelectList(ZoneDropDownList.Zones, "Id", "Zone");
            int totalRecords = labs.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            labs = labs.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            ViewBag.msg = TempData["msg"] as string;
            return View(labs);
        }

        //Lab Details
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var lab = await _unitOfWork.labs.GetLab(id);
                if (lab == null)
                    return View("NotFound");
                var labVM = new LabViewModel
                {
                    Id = id,
                    Name = lab.Name,
                    Email = lab.User.Email,
                    PhoneNumber = lab.User.PhoneNumber,
                    Location = lab.User.Location,
                    Price = lab.User.Price,
                    Zone = lab.User.Zone,
                    About = lab.User.About,
                    StartTime = lab.User.StartTime,
                    EndTime = lab.User.EndTime,
                    ProfileImg = lab.LabImage,
                };
                return View(labVM);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        //Edit Lab
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var lab = await _unitOfWork.labs.GetLab(id);
                if (lab == null)
                    return View("NotFound");

                var labVM = new LabViewModel
                {
                    Id = id,
                    Name = lab.Name,
                    Email = lab.User.Email,
                    PhoneNumber = lab.User.PhoneNumber,
                    Location = lab.User.Location,
                    Price = lab.User.Price,
                    Zone = lab.User.Zone,
                    About = lab.User.About,
                    StartTime = lab.User.StartTime,
                    EndTime = lab.User.EndTime,
                    ProfileImg = lab.LabImage,
                };
                return View(labVM);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, LabViewModel model)
        {
            try
            {
                var lab = await _unitOfWork.labs.GetLab(id);
                if (lab == null)
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

                lab.User.Name = model.Name;
                lab.User.PhoneNumber = model.PhoneNumber;
                lab.User.Location = model.Location;
                lab.User.Zone = model.Zone;
                lab.User.StartTime = model.StartTime;
                lab.User.EndTime = model.EndTime;
                lab.User.Price = model.Price;
                lab.User.Email = model.Email;
                lab.User.About = model.About;
                lab.User.ProfileImg = model.ProfileImg;
                lab.Name = model.Name;
                lab.PhoneNumber = model.PhoneNumber;
                lab.Location = model.Location;
                lab.Zone = model.Zone;
                lab.StartTime = model.StartTime;
                lab.EndTime = model.EndTime;
                lab.Price = model.Price;
                lab.LabImage = model.ProfileImg;

                _context.Labs.Update(lab);
                await _unitOfWork.Complete();
                _toastNotification.AddSuccessToastMessage("Lab Updated successfully");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                _toastNotification.AddErrorToastMessage("Lab Updated Failed");
                return View();
            }
        }

        //Delete Lab 
        public async Task<IActionResult> Delete(int id)
        {
            var lab = _unitOfWork.labs.Get_Lab(id);
            if (lab != null)
            {
                //_labService.Delete(lab);
                _unitOfWork.labs.Delete(lab);
                await _unitOfWork.Complete();
                _toastNotification.AddSuccessToastMessage($"Prescription with ID {id} removed successfully");
            }
            return RedirectToAction("Index");
        }

        //Delete lab
        public IActionResult DeleteLab(int id)
        {
            var isDeleted = _unitOfWork.labs.DeleteLab(id);
            return isDeleted ? Ok() : BadRequest();
        }
    }
}
