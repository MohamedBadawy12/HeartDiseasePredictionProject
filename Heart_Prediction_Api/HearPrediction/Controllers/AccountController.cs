using Database.Entities;
using HearPrediction.Api.DTO;
using HearPrediction.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HearPrediction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileService _fileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JWT _jwt;
        //private readonly Repositories.IMailService _mailService;

        public AccountController(IAuthService authService, SignInManager<ApplicationUser> signInManager,
             IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, /*Repositories.IMailService mailServices,*/ JWT jwt, IFileService fileRepository)
        {
            _authService = authService;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            //_mailService = mailServices;
            _fileRepository = fileRepository;
            _jwt = jwt;
        }

        //Register of patient
        [HttpPost("registerPatient")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterUserAsync(model);
            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var confirmationLink = Url.Action(nameof(ConfirmationOfEmail),"Authentication",new ConfirmEmailDto { Token = token,Email = user.Email});
            //var message = new MailRequestDto(user.Email, "Confirmation Email Link",confirmationLink);
            //_mailServices.SendEmail(message);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        //Register of doctor
        [HttpPost("registerDoctor")]
        //[AllowAnonymous]
        public async Task<IActionResult> RegisterDoctorAsync([FromBody] RegisterDoctorDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterDoctorAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        //Register of medicalAnalyst
        [HttpPost("registerMedicalAnalyst")]
        //[AllowAnonymous]
        public async Task<IActionResult> RegisterMedicalAnalystAsync([FromBody] RegisterMedicalAnalystDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterMedicalAnalystAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        //Register of receptionist
        [HttpPost("registerReceptionist")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterReceptionistAsync([FromBody] RegisterReciptionistDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterReceptionistAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        //Register of Lab
        [HttpPost("AddLab")]
        [AllowAnonymous]
        public async Task<IActionResult> AddLabAsync([FromQuery] RegisterLabtDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterLabAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        //Login  
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginTokenAsync([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }

        //Get Profile Details of users
        [HttpGet("GetProfileDetails")]
        [AllowAnonymous]
        public async Task<IActionResult> Profile()
        {
            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                ProfileUpdateDto model = new ProfileUpdateDto()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    BirthDate = user.BirthDate,
                    ProfileImg = user.ProfileImg,
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                    Name = user.Name,
                    Location = user.Location,
                    Zone = user.Zone,
                    About = user.About,
                    Price = user.Price,
                };
                return Ok(model);
            }
            return BadRequest("Please Login to get your profile");
        }

        //Update profile of user
        [HttpPut("Update")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateProfile([FromQuery] ProfileUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByNameAsync(userName);

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
            if (user != null)
            {
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.BirthDate = model.BirthDate;
                user.Gender = model.Gender;
                user.ProfileImg = model.ProfileImg;
                user.Name = model.Name;
                user.Location = model.Location;
                user.Zone = model.Zone;
                user.About = model.About;
                user.Price = model.Price;
                user.Name = model.Name;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return StatusCode(500, "Failed to update user profile.");
                }
                return Ok("Profile updated successfully");
            }
            return NotFound("User not found.");

        }
        //[HttpGet("TestEmail")]
        //[AllowAnonymous]
        //public IActionResult TestEmail()
        //{
        //	var message = new MailRequestDto("m.badawy2442002@gmail.com"
        //	, "Test", "<h1>Welcome to our Website</h1>");
        //	_mailServices.SendEmail(message);
        //	return StatusCode(StatusCodes.Status200OK,
        //		new Responses { Status = "Success", Message = "Email sent successfuly" });

        //}
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto confirm)
        {
            var user = await _userManager.FindByNameAsync(confirm.Email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, confirm.Token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK,
                        new Responses { Status = "Success", Message = "Email Verfied Successfully" });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                        new Responses { Status = "Error", Message = "This User Doesn't exist!" });
        }

        [HttpPost("ForgetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(ForgetPassword model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgetPasswordLink = Url.Action(nameof(Resetpassword), "Authentication", new { token, email = user.Email }
                , Request.Scheme);
                //var message = new MailRequestViewModel(user.Email, "Forget Password Link", forgetPasswordLink);
                //_mailService.SendEmail(message);
                return StatusCode(StatusCodes.Status200OK,
                    new Responses
                    {
                        Status = "Success",
                        Message = $"Password changed is sent on Email " +
                    $"{user.Email}.please open your Email & click the link"
                    });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                    new Responses
                    {
                        Status = "Error",
                        Message = $"Couldn't sent link to email, Please try again."
                    });
        }

        [HttpGet("Resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> Resetpassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };
            return Ok(new { model });
        }

        [HttpPost("Resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> Resetpassword(ResetPassword resetPassword)
        {
            var user = await _userManager.FindByNameAsync(resetPassword.Email);
            if (user != null)
            {
                var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
                if (resetPassResult.Succeeded)
                {
                    foreach (var error in resetPassResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Ok(ModelState);
                }
                return StatusCode(StatusCodes.Status200OK,
                    new Responses
                    {
                        Status = "Success",
                        Message = $"Password changed is sent on Email "
                    });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                    new Responses
                    {
                        Status = "Error",
                        Message = $"Couldn't sent link to email, Please try again."
                    });
        }

        [HttpGet("refreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("revokeToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _authService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token is invalid!");

            return Ok();
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            if (refreshToken == null)
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }
            //var escapedRefreshToken = Uri.EscapeDataString(refreshToken);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
