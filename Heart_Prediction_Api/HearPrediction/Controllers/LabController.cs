using Database.Entities;
using HearPrediction.Api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace HearPrediction.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class LabController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IFileService _fileRepository;
        public LabController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager
            , AppDbContext context, IFileService fileRepository)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _context = context;
            _fileRepository = fileRepository;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var labs = await _unitOfWork.labs.GetLabs();
            return Ok(labs);
        }

        //Lab Details
        [HttpGet("GetLabDetails")]
        public async Task<IActionResult> Details(int id)
        {
            var lab = await _unitOfWork.labs.GetLab(id);
            if (lab == null)
                return NotFound($"No lab was found with Id: {id}");
            var labVM = new LabDTO
            {
                Id = id,
                Email = lab.User.Email,
                Name = lab.Name,
                PhoneNumber = lab.PhoneNumber,
                Location = lab.Location,
                Price = lab.Price,
                ProfileImg = lab.LabImage,
            };
            return Ok(labVM);
        }

        //Edit Lab
        [HttpPut("EditLab")]
        public async Task<IActionResult> Edit(int id, LabDTO model)
        {
            var lab = await _unitOfWork.labs.GetLab(id);
            if (lab == null)
                return NotFound($"No lab was found with Id: {id}");

            var path = model.ProfileImg;
            if (model.ImageFile?.Length > 0)
            {
                _fileRepository.DeleteImage(path);
                path = await _fileRepository.UploadAsync(model.ImageFile, "/Upload/");
                if (path == "An Problem occured when creating file")
                {
                    return BadRequest("An Problem occured when creating file");
                }
            }
            model.ProfileImg = path;

            lab.User.Name = model.Name;
            lab.User.PhoneNumber = model.PhoneNumber;
            lab.User.Location = model.Location;
            lab.User.Price = model.Price;
            lab.User.Email = model.Email;
            lab.User.ProfileImg = model.ProfileImg;
            lab.Name = model.Name;
            lab.PhoneNumber = model.PhoneNumber;
            lab.Location = model.Location;
            lab.Price = model.Price;
            lab.LabImage = model.ProfileImg;

            await _unitOfWork.Complete();
            return Ok(model);
        }

        //Delete Doctor
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lab = _unitOfWork.labs.Get_Lab(id);
            if (lab == null)
                return NotFound($"No lab was found with Id: {id}");
            try
            {
                string path = "";
                path = lab.LabImage;
                _unitOfWork.labs.Delete(lab);
                _fileRepository.DeleteImage(path);
                await _unitOfWork.Complete();
                return Ok($"LAb with ID {id} removed successfully");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }
    }
}
