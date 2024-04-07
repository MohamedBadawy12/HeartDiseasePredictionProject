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
	public class PatientController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public PatientController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}

		//Get All Patients from db
		[HttpGet]
		public async Task<IActionResult> GetAllPatients()
		{
			var pateints = await _unitOfWork.Patients.GetPatients();
			return Ok(pateints);
		}

		//Get patient details from db
		[HttpGet("GetPatientBySSN")]
		public async Task<IActionResult> GetPatientDetails(long ssn)
		{
			var pateint = await _unitOfWork.Patients.GetPatient(ssn);
			if (pateint == null)
				return NotFound($"No patient was found with SSN: {ssn}");
			var patientDetail = new UserFormDTO
			{
				FirstName = pateint.User.FirstName,
				LastName = pateint.User.LastName,
				SSN = pateint.SSN,
				Insurance_No = pateint.Insurance_No,
				BirthDate = pateint.User.BirthDate,
				Email = pateint.User.Email,
				Gender = pateint.User.Gender,
				PhoneNumber = pateint.User.PhoneNumber,
				ProfileImg = pateint.User.ProfileImg,
			};
			return Ok(patientDetail);
		}

		//Search For Patient
		[HttpGet("Search")]
		public async Task<IActionResult> SearchForPatient([FromQuery] string search)
		{
			try
			{
				var result = await _unitOfWork.Patients.FilterPatients(search);
				return Ok(result);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error in search for data");
			}
		}

		//Edit Patient 
		[HttpPut("EditPatient")]
		public async Task<IActionResult> EditPatient(long ssn, [FromBody] UserFormDTO model)
		{
			var patient = await _unitOfWork.Patients.GetPatient(ssn);
			if (patient == null)
				return NotFound($"No patient was found with SSN: {ssn}");

			string wwwRootPath = _webHostEnvironment.WebRootPath;
			string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
			string extension = Path.GetExtension(model.ImageFile.FileName);
			model.ProfileImg = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
			string path = Path.Combine(wwwRootPath + "/Upload", fileName);

			using (var fileStream = new FileStream(path, FileMode.Create))
			{
				await model.ImageFile.CopyToAsync(fileStream);
			}

			patient.User.PhoneNumber = model.PhoneNumber;
			patient.SSN = model.SSN;
			patient.Insurance_No = model.Insurance_No;
			patient.User.SSN = model.SSN;
			patient.User.Insurance_No = model.Insurance_No;
			patient.User.FirstName = model.FirstName;
			patient.User.Email = model.Email;
			patient.User.LastName = model.LastName;
			patient.User.Gender = model.Gender;
			patient.User.BirthDate = model.BirthDate;
			patient.User.ProfileImg = model.ProfileImg;

			await _unitOfWork.Complete();
			return Ok(patient);
		}

		//Delete Patient
		[HttpDelete("{ssn}")]
		public async Task<IActionResult> Delete(long ssn)
		{
			var patient = _unitOfWork.Patients.Get_Patient(ssn);
			if (patient == null)
				return NotFound($"No patient was found with SSN: {ssn}");
			//var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Upload", patient.User.ProfileImg);
			try
			{
				//if (System.IO.File.Exists(imagePath))
				//	System.IO.File.Delete(imagePath);
				_unitOfWork.Patients.Remove(patient);
				await _unitOfWork.Complete();
				return Ok(patient);
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
			}
		}
	}
}
