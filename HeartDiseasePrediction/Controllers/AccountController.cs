using Database.Entities;
using HeartDiseasePrediction.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NToastNotify;
using Repositories;
using Repositories.Interfaces;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HeartDiseasePrediction.Controllers
{
    public class AccountController : Controller
    {
        private readonly IToastNotification _toastNotification;
        Uri baseAddress = new Uri("https://localhost:44304/api");
        HttpClient _client;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IFileRepository _fileRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        public AccountController(IToastNotification toastNotification, UserManager<ApplicationUser> userManger,
            SignInManager<ApplicationUser> signInManager, AppDbContext context, IFileRepository fileRepository,
            IUnitOfWork unitOfWork)
        {
            _toastNotification = toastNotification;
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            _userManager = userManger;
            _signInManager = signInManager;
            _fileRepository = fileRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }
        //Login
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginViewModel)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
                    if (user != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                        if (result.Succeeded)
                        {
                            _toastNotification.AddSuccessToastMessage("Login Success");
                            return RedirectToAction("Index", "Home");
                        }
                        _toastNotification.AddErrorToastMessage("Email or Password is Incorrect");
                        TempData["Error"] = "Wrong credentials. please,try again!";
                        return View(loginViewModel);
                    }
                    _toastNotification.AddErrorToastMessage("Email or Password is Incorrect");
                    TempData["Error"] = "Wrong credentials. please,try again!";
                }
                return View(loginViewModel);
            }
            catch (Exception ex)
            {
                TempData["errorMesssage"] = ex.Message;
                _toastNotification.AddErrorToastMessage("Email or Password is Incorrect");
                return View(loginViewModel);
            }
        }
        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress +
                "/Account/ForgetPassword", content);
            if (response.IsSuccessStatusCode)
            {
                //TempData["successMesssage"] = "Password changed successfully.";
                //_toastNotification.AddSuccessToastMessage("Password changed successfully.");
                return RedirectToAction(nameof(Login));
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Error, please Check the Email");
                return View();
            }
        }

        public async Task<IActionResult> Resetpassword(string token, string email)
        {
            var model = new ResetPasswordViewModel { Code = token, Email = email };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resetpassword(ResetPasswordViewModel model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            //var content = new FormUrlEncodedContent(new[]
            //{
            //    new KeyValuePair<string, string>("email", model.Email),
            //    new KeyValuePair<string, string>("password", model.Password),
            //    new KeyValuePair<string, string>("confirmPassword", model.ConfirmPassword),
            //});
            HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress +
                "/Account/Resetpassword", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["successMesssage"] = "Password changed successfully.";
                _toastNotification.AddSuccessToastMessage("Password changed successfully.");
                return RedirectToAction(nameof(Login));
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Error in changing password");
                return View();
            }

        }

        //Logout 
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> RegisterOfUser()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterOfUser(RegisterUserVM model)
        {

            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return View(model);

            var user = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                BirthDate = model.BirthDate,
                PhoneNumber = model.PhoneNumber,
                SSN = model.SSN,
                Insurance_No = model.Insurance_No,
                //TwoFactorEnabled = true,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                var patient = new Patient
                {
                    UserId = user.Id,
                    SSN = model.SSN,
                    Insurance_No = model.Insurance_No,
                };

                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Register User successfully.");
                return View("CompletedSuccessfully");
            }
            _toastNotification.AddErrorToastMessage("Register User Failed");
            return View(model);
        }
        public async Task<IActionResult> RegisterOfDoctor()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterOfDoctor(RegisterDoctorVM model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return View(model);

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                Name = model.Name,
                LastName = model.LastName,
                Gender = model.Gender,
                BirthDate = model.BirthDate,
                PhoneNumber = model.PhoneNumber,
                Price = model.Price,
                Location = model.Location,
                //TwoFactorEnabled = true,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Doctor");
                var doctor = new Doctor
                {
                    UserId = user.Id,
                    IsAvailable = true,
                    Name = model.Name,
                    Location = model.Location,
                    Price = model.Price,
                };
                await _context.Doctors.AddAsync(doctor);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Doctor Created successfully.");
                return RedirectToAction("Index", "Doctor");
            }
            _toastNotification.AddErrorToastMessage("Register Doctor Failed");
            return View(model);
        }
        public async Task<IActionResult> RegisterOfMedicalAnalyst()
        {
            var labDropDownList = await _unitOfWork.labs.GetLabDropDownsValues();
            ViewBag.Lab = new SelectList(labDropDownList.labs, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterOfMedicalAnalyst(RegisterMedicalAnalystVM model)
        {
            if (!ModelState.IsValid)
            {
                var labDropDownList = await _unitOfWork.labs.GetLabDropDownsValues();
                ViewBag.Lab = new SelectList(labDropDownList.labs, "Id", "Name");
                return View(model);
            }
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return View(model);

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                BirthDate = model.BirthDate,
                PhoneNumber = model.PhoneNumber,
                //TwoFactorEnabled = true,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "MedicalAnalyst");
                var medicalAnalyst = new MedicalAnalyst
                {
                    UserId = user.Id,
                    LabId = model.Lab
                };
                await _context.MedicalAnalysts.AddAsync(medicalAnalyst);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Medical Analyst Created successfully.");
                return RedirectToAction("Index", "MedicalAnalyst");
            }
            _toastNotification.AddErrorToastMessage("Register Medical Analyst Failed");
            return View(model);
        }
        public async Task<IActionResult> RegisterOfReciptionist()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterOfReciptionist(RegisterReciptionistVM model)
        {

            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return View(model);

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                BirthDate = model.BirthDate,
                PhoneNumber = model.PhoneNumber,
                //TwoFactorEnabled = true,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                var reciptionist = new Reciptionist
                {
                    UserId = user.Id,
                };
                await _context.Reciptionists.AddAsync(reciptionist);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage("Reciptionist Created successfully.");
                return RedirectToAction("Index", "Reciptionist");
            }
            _toastNotification.AddErrorToastMessage("Register Reciptionist Failed");
            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(ApplicationUser model, string code)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (model.Id == null || code == null)
            {
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return View("ForgotPasswordConfirmation");
                }

                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> Update()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return View("Login");
            var user = _userManager.FindByIdAsync(userId).Result;
            return View(user);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Update(ApplicationUser model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound();

            user.Id = model.Id;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;
            user.PasswordHash = model.PasswordHash;
            user.Email = model.Email;
            user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
            user.BirthDate = model.BirthDate;
            user.EmailConfirmed = model.EmailConfirmed;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Name = model.Name;
            user.Gender = model.Gender;
            user.ConcurrencyStamp = model.ConcurrencyStamp;
            user.SSN = model.SSN;
            user.ImageFile = model.ImageFile;
            user.ProfileImg = model.ProfileImg;
            user.Price = model.Price;
            user.LockoutEnd = model.LockoutEnd;
            user.LockoutEnabled = model.LockoutEnabled;
            user.NormalizedUserName = model.NormalizedUserName;
            user.Location = model.Location;
            user.Insurance_No = model.Insurance_No;
            user.NormalizedEmail = model.NormalizedEmail;
            user.SecurityStamp = model.SecurityStamp;
            user.TwoFactorEnabled = model.TwoFactorEnabled;

            var result = await _userManager.UpdateAsync(model);
            if (result.Succeeded)
            {
                _toastNotification.AddSuccessToastMessage("Account Updated Successfully");
                return RedirectToAction("Update");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return BadRequest("Register Or Login Please");
            var user = await _userManager.FindByIdAsync(userId);
            string email = User.FindFirstValue(ClaimTypes.Email);
            var model = new ApplicationUser
            {
                Id = userId,
                Email = email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Name = user.Name,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber,
                ProfileImg = user.ProfileImg,
                Insurance_No = user.Insurance_No,
                SSN = user.SSN,
                Location = user.Location,
                Price = user.Price,
                BirthDate = user.BirthDate,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(EditAccountProfile model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return BadRequest("Register Or Login Please");
            string email = User.FindFirstValue(ClaimTypes.Email);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            //_fileRepository.SaveImage(model.ImageFile);
            var userModel = new ApplicationUser
            {
                Id = userId,
                Email = email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                SSN = model.SSN,
                Insurance_No = model.Insurance_No,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                Location = model.Location,
                Price = model.Price,
                ProfileImg = model.ProfileImg,
            };
            //if (userRole == "User")
            //{
            //    var users = new Patient
            //    {
            //        UserId = userId,
            //        SSN = model.SSN,
            //        Insurance_No = model.Insurance_No
            //    };
            //}
            //if (userRole == "Doctor")
            //{
            //    var users = new Doctor
            //    {
            //        UserId = userId,
            //        Price = model.Price,
            //        Name = model.Name,
            //        Location = model.Location,
            //    };
            //}
            _context.Users.Update(userModel);
            await _unitOfWork.Complete();
            //_context.Patients.Update(users);
            //await _unitOfWork.Complete();
            _toastNotification.AddSuccessToastMessage("Account Updated Successfully");
            return View(userModel);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
