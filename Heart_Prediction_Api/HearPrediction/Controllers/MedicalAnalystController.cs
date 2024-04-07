using HearPrediction.Api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HearPrediction.Api.Controllers
{
	[Authorize(Roles = "Admin")]
	[Route("api/[controller]")]
	[ApiController]
	public class MedicalAnalystController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public MedicalAnalystController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}

		//Get All MedicalAnalysts from db
		[HttpGet]
		public async Task<IActionResult> GetAllMedicalAnalyst()
		{
			try
			{
				var medicalAnalyst = await _unitOfWork.medicalAnalysts.GetMedicalAnalysts();
				return Ok(medicalAnalyst);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message + "  " + ex.StackTrace);
			}
		}

		//Get MedicalAnalyst Details db
		[HttpGet("GetMedicalAnalystById")]
		public async Task<IActionResult> GetMedicalAnalystDetails(int id)
		{
			var medicalAnalyst = await _unitOfWork.medicalAnalysts.GetMedicalAnalyst(id);
			if (medicalAnalyst == null)
				return NotFound($"No Medical Analyst was found with Id: {id}");

			var medicalAnalystDetail = new MedicalAnalystFormDTO
			{
				FirstName = medicalAnalyst.User.FirstName,
				LastName = medicalAnalyst.User.LastName,
				BirthDate = medicalAnalyst.User.BirthDate,
				Email = medicalAnalyst.User.Email,
				PhoneNumber = medicalAnalyst.User.PhoneNumber,
				Gender = medicalAnalyst.User.Gender,
				ProfileImg = medicalAnalyst.User.ProfileImg,
				LabId = medicalAnalyst.LabId,
				labName = medicalAnalyst.Lab.Name,
			};
			return Ok(medicalAnalystDetail);
		}
		//Search For Medical Analyst
		[HttpGet("Search")]
		public async Task<IActionResult> SearchForMedicalAnalyst([FromQuery] string search)
		{
			try
			{
				var result = await _unitOfWork.medicalAnalysts.FilterMedicalAnalyst(search);
				return Ok(result);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error in searchin for data");
			}
		}

		//Edit MedicalAnalyst
		[HttpPut("EditMedicalAnalyst")]
		public async Task<IActionResult> EditMedicalAnalyst(int id, [FromBody] MedicalAnalystFormDTO model)
		{
			var medicalAnalyst = await _unitOfWork.medicalAnalysts.GetMedicalAnalyst(id);
			if (medicalAnalyst == null)
				return NotFound($"No Medical Analyst was found with Id: {id}");

			string wwwRootPath = _webHostEnvironment.WebRootPath;
			string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
			string extension = Path.GetExtension(model.ImageFile.FileName);
			model.ProfileImg = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
			string path = Path.Combine(wwwRootPath + "/Upload", fileName);

			using (var fileStream = new FileStream(path, FileMode.Create))
			{
				await model.ImageFile.CopyToAsync(fileStream);
			}

			medicalAnalyst.User.PhoneNumber = model.PhoneNumber;
			medicalAnalyst.User.Email = model.Email;
			medicalAnalyst.User.FirstName = model.FirstName;
			medicalAnalyst.User.LastName = model.LastName;
			medicalAnalyst.User.BirthDate = model.BirthDate;
			medicalAnalyst.User.Gender = model.Gender;
			medicalAnalyst.User.ProfileImg = model.ProfileImg;
			medicalAnalyst.LabId = model.LabId;

			await _unitOfWork.Complete();
			return Ok(medicalAnalyst);
		}

		//Delete MedicalAnalyst
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var medicalAnalyst = _unitOfWork.medicalAnalysts.Get_MedicalAnalyst(id);
			if (medicalAnalyst == null)
				return NotFound($"No Medical Analyst was found with Id: {id}");
			//var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Upload", medicalAnalyst.User.ProfileImg);
			try
			{
				//if (System.IO.File.Exists(imagePath))
				//	System.IO.File.Delete(imagePath);
				_unitOfWork.medicalAnalysts.Remove(medicalAnalyst);
				await _unitOfWork.Complete();
				return Ok(medicalAnalyst);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
			}
		}
	}
}
