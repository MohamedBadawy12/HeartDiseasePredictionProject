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
	public class ReciptionistController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ReciptionistController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
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

			string wwwRootPath = _webHostEnvironment.WebRootPath;
			string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
			string extension = Path.GetExtension(model.ImageFile.FileName);
			model.ProfileImg = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
			string path = Path.Combine(wwwRootPath + "/Upload", fileName);

			using (var fileStream = new FileStream(path, FileMode.Create))
			{
				await model.ImageFile.CopyToAsync(fileStream);
			}

			reciptionist.User.PhoneNumber = model.PhoneNumber;
			reciptionist.User.Email = model.Email;
			reciptionist.User.FirstName = model.FirstName;
			reciptionist.User.LastName = model.LastName;
			reciptionist.User.Gender = (Database.Enums.Gender)model.Gender;
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
			//var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Upload", reciptionist.User.ProfileImg);
			try
			{
				//if (System.IO.File.Exists(imagePath))
				//	System.IO.File.Delete(imagePath);
				_unitOfWork.reciptionists.Remove(reciptionist);
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
