using Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Repositories;
using System;
using System.Threading.Tasks;

namespace HeartDiseasePrediction.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LabController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        public LabController(IToastNotification toastNotification, AppDbContext context
            , IUnitOfWork unitOfWork)
        {
            _toastNotification = toastNotification;
            _unitOfWork = unitOfWork;
            _context = context;
        }
        //Lab List
        public async Task<IActionResult> Index()
        {
            var labs = await _unitOfWork.labs.GetLabs();
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
                var labVM = new Lab
                {
                    Id = id,
                    Name = lab.Name,
                    PhoneNumber = lab.PhoneNumber,
                };
                return View(labVM);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        //Create Lab
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Lab model)
        {
            if (!ModelState.IsValid)
                return View(model);
            try
            {
                //await _labService.AddAsync(model);
                await _unitOfWork.labs.AddAsync(model);
                await _unitOfWork.Complete();
                TempData["msg"] = "Lab Created successfully";
                _toastNotification.AddSuccessToastMessage("Lab Created successfully");
                return View("CompletedSuccessfully");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                _toastNotification.AddErrorToastMessage("An error occurred while saving the prescription.");
                return View(model);
            }
        }


        //Edit Lab
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var lab = await _unitOfWork.labs.GetLab(id);
                if (lab == null)
                    return View("NotFound");
                var labVM = new Lab
                {
                    Id = id,
                    Name = lab.Name,
                    PhoneNumber = lab.PhoneNumber,
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
        public async Task<IActionResult> Edit(int id, Lab model)
        {
            try
            {
                var lab = await _unitOfWork.labs.GetLab(id);
                if (lab == null)
                    return View("NotFound");

                lab.Name = model.Name;
                lab.PhoneNumber = model.PhoneNumber;

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
