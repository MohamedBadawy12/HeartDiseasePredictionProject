using HearPrediction.Api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class DoctorController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileRepository;
        public DoctorController(IUnitOfWork unitOfWork, IFileService fileRepository)
        {
            _unitOfWork = unitOfWork;
            _fileRepository = fileRepository;
        }
        //Get All Doctors from db
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _unitOfWork.Doctors.GetDoctors();
            return Ok(doctors);
        }

        //Get Doctor details from db
        [HttpGet("GetDoctorById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDoctorDetails(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetDoctor(id);
            if (doctor == null)
                return NotFound($"No doctor was found with Id: {id}");

            var DoctorDetail = new DoctorFormDTO
            {
                FirstName = doctor.User.FirstName,
                LastName = doctor.User.LastName,
                BirthDate = doctor.User.BirthDate,
                Name = doctor.User.Name,
                Location = doctor.User.Location,
                Zone = doctor.User.Zone,
                Price = doctor.User.Price,
                Email = doctor.User.Email,
                Gender = doctor.User.Gender,
                About = doctor.User.About,
                StartTime = doctor.User.StartTime,
                EndTime = doctor.User.EndTime,
                PhoneNumber = doctor.User.PhoneNumber,
                ProfileImg = doctor.User.ProfileImg,
            };
            return Ok(DoctorDetail);
        }

        //Search for doctor
        [HttpGet("Search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchForDoctor([FromQuery] string search, string location)
        {
            try
            {
                var result = await _unitOfWork.Doctors.FilterDoctors(search, location);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in searchin for data");
            }
        }

        //Edit Doctor 
        [HttpPut("EditDoctor")]
        public async Task<IActionResult> EditDoctor(int id, [FromForm] DoctorFormDTO model)
        {
            var doctor = await _unitOfWork.Doctors.GetDoctor(id);
            if (doctor == null)
                return NotFound($"No doctor was found with Id: {id}");

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

            doctor.User.PhoneNumber = model.PhoneNumber;
            doctor.User.Email = model.Email;
            doctor.User.Name = model.Name;
            doctor.User.Location = model.Location;
            doctor.User.Price = model.Price;
            doctor.User.FirstName = model.FirstName;
            doctor.User.LastName = model.LastName;
            doctor.User.BirthDate = model.BirthDate;
            doctor.User.About = model.About;
            doctor.User.Gender = model.Gender;
            doctor.User.Zone = model.Zone;
            doctor.User.StartTime = model.StartTime;
            doctor.User.EndTime = model.EndTime;
            doctor.User.ProfileImg = model.ProfileImg;
            doctor.Zone = model.Zone;
            doctor.Location = model.Location;
            doctor.Price = model.Price;

            await _unitOfWork.Complete();
            return Ok(doctor);
        }

        //Delete Doctor
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = _unitOfWork.Doctors.Get_Doctor(id);
            if (doctor == null)
                return NotFound($"No doctor was found with Id: {id}");
            try
            {
                string path = "";
                path = doctor.User.ProfileImg;
                _unitOfWork.Doctors.Delete(doctor);
                _fileRepository.DeleteImage(path);
                await _unitOfWork.Complete();
                return Ok($"Doctor with ID {id} removed successfully");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }
    }
}
