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
    public class PatientController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileRepository;
        public PatientController(IUnitOfWork unitOfWork, IFileService fileRepository)
        {
            _unitOfWork = unitOfWork;
            _fileRepository = fileRepository;
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

            try
            {
                string path = "";
                path = patient.User.ProfileImg;
                _unitOfWork.Patients.Remove(patient);
                _fileRepository.DeleteImage(path);
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
