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
    public class ReciptionistController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileRepository;
        public ReciptionistController(IUnitOfWork unitOfWork, IFileService fileRepository)
        {
            _unitOfWork = unitOfWork;
            _fileRepository = fileRepository;
        }
        //Get all Reciptionists from db
        [HttpGet]
        public async Task<IActionResult> GetAllReciptionists()
        {
            var reciptionist = await _unitOfWork.reciptionists.GetReciptionists();
            return Ok(reciptionist);
        }

        //Get Reciptionist details from db
        [HttpGet("GetReciptionistById")]
        public async Task<IActionResult> GetReciptionistDetails(int id)
        {
            var reciptionist = await _unitOfWork.reciptionists.GetReciptionist(id);
            if (reciptionist == null)
                return NotFound($"No Reciptionist was found with Id: {id}");

            var reciptionistDetail = new ReciptionistFormDTO
            {
                FirstName = reciptionist.User.FirstName,
                LastName = reciptionist.User.LastName,
                BirthDate = reciptionist.User.BirthDate,
                Email = reciptionist.User.Email,
                Gender = reciptionist.User.Gender,
                PhoneNumber = reciptionist.User.PhoneNumber,
                ProfileImg = reciptionist.User.ProfileImg,
            };
            return Ok(reciptionistDetail);
        }

        //Search For Reciptionist
        [HttpGet("Search")]
        public async Task<IActionResult> SearchForReciptionist([FromQuery] string search)
        {
            try
            {
                var result = await _unitOfWork.reciptionists.FilterReciptionist(search);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in search for data");
            }
        }

        //Edit the Reciptionist 
        [HttpPut("EditReciptionist")]
        public async Task<IActionResult> EditReciptionist(int id, [FromBody] ReciptionistFormDTO model)
        {
            var reciptionist = await _unitOfWork.reciptionists.GetReciptionist(id);
            if (reciptionist == null)
                return NotFound($"No Reciptionist was found with Id: {id}");

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

            reciptionist.User.PhoneNumber = model.PhoneNumber;
            reciptionist.User.Email = model.Email;
            reciptionist.User.FirstName = model.FirstName;
            reciptionist.User.LastName = model.LastName;
            reciptionist.User.Gender = model.Gender;
            reciptionist.User.BirthDate = model.BirthDate;
            reciptionist.User.ProfileImg = model.ProfileImg;

            await _unitOfWork.Complete();
            return Ok(reciptionist);
        }

        //Delete the Reciptionist from db
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reciptionist = _unitOfWork.reciptionists.Get_Reciptionist(id);
            if (reciptionist == null)
                return NotFound($"No Reciptionist was found with Id: {id}");
            try
            {
                string path = "";
                path = reciptionist.User.ProfileImg;
                _unitOfWork.reciptionists.Remove(reciptionist);
                _fileRepository.DeleteImage(path);
                await _unitOfWork.Complete();
                return Ok(reciptionist);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }
    }
}
