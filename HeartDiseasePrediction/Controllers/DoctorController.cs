using Database.Entities;
using HeartDiseasePrediction.ViewModel;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public class DoctorController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly IFileService _fileRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DoctorController(IToastNotification toastNotification, AppDbContext context
            , IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork, IFileService fileRepository)
        {
            _toastNotification = toastNotification;
            _unitOfWork = unitOfWork;
            _context = context;
            _fileRepository = fileRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        //get all doctors in list
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var doctors = await _unitOfWork.Doctors.GetDoctors();
            int totalRecords = doctors.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            doctors = doctors.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(doctors);
        }
        //Search 
        [HttpPost]
        public async Task<IActionResult> Index(string search, string location, int currentPage = 1)
        {
            var doctorViewModel = await _unitOfWork.Doctors.FilterDoctors(search, location);
            int totalRecords = doctorViewModel.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            doctorViewModel = doctorViewModel.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(doctorViewModel);
        }
        [AllowAnonymous]
        public async Task<IActionResult> DoctorsList(int currentPage = 1)
        {
            var doctors = await _unitOfWork.Doctors.GetDoctors();
            var ZoneDropDownList = await _unitOfWork.labs.GetLabZoneDropDownsValues();
            ViewBag.Zone = new SelectList(ZoneDropDownList.Zones, "Id", "Zone");
            int totalRecords = doctors.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            doctors = doctors.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
            return View(doctors);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> DoctorsList(string search, string zone, int currentPage = 1)
        {
            var ZoneDropDownList = await _unitOfWork.labs.GetLabZoneDropDownsValues();
            ViewBag.Zone = new SelectList(ZoneDropDownList.Zones, "Id", "Zone");
            var doctors = await _unitOfWork.Doctors.FilterDoctors(search, zone);
            int totalRecords = doctors.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            doctors = doctors.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPrevious = currentPage > 1;
            ViewBag.HasNext = currentPage < totalPages;
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
                    Name = doctor.User.Name,
                    Location = doctor.User.Location,
                    Price = doctor.User.Price,
                    Zone = doctor.User.Zone,
                    About = doctor.User.About,
                    //Name = doctor.Name,
                    //Location = doctor.Location,
                    //Price = doctor.Price,
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
                    Name = doctor.User.Name,
                    Location = doctor.User.Location,
                    Price = doctor.User.Price,
                    Zone = doctor.User.Zone,
                    About = doctor.User.About,
                    ProfileImg = doctor.User.ProfileImg,
                    //Name = doctor.Name,
                    //Location = doctor.Location,
                    //Price = doctor.Price,
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

                doctor.User.PhoneNumber = model.PhoneNumber;
                doctor.User.Email = model.Email;
                doctor.User.FirstName = model.FirstName;
                doctor.User.LastName = model.LastName;
                doctor.User.BirthDate = model.BirthDate;
                doctor.User.Name = model.Name;
                doctor.User.Location = model.Location;
                doctor.User.About = model.About;
                doctor.User.Zone = model.Zone;
                doctor.User.Price = model.Price;
                doctor.User.Gender = model.Gender;
                doctor.Name = model.Name;
                doctor.Location = model.Location;
                doctor.Zone = model.Zone;
                doctor.Price = model.Price;
                doctor.User.ProfileImg = model.ProfileImg;

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
            try
            {
                string path = "";
                path = doctor.User.ProfileImg;
                _unitOfWork.Doctors.Delete(doctor);
                _fileRepository.DeleteImage(path);
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
