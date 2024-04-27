using Database.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HearPrediction.Api.DTO
{
    public class RegisterDoctorDTO
    {
        [Display(Name = "First Name"), StringLength(100)]
        [Required(ErrorMessage = "First Name Is Required")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name"), StringLength(100)]
        [Required(ErrorMessage = "Last Name Is Required")]
        public string LastName { get; set; }
        [Display(Name = "Location"), StringLength(200)]
        public string? Location { get; set; }
        [Display(Name = "Zone"), StringLength(100)]
        public string? Zone { get; set; }
        [Display(Name = "Name"), StringLength(150)]
        public string? Name { get; set; }
        [Display(Name = "Price")]
        public string? Price { get; set; }
        [Display(Name = "About")]
        public string? About { get; set; }
        [Display(Name = "Start Time")]
        public string? StartTime { get; set; }
        [Display(Name = "End Time")]
        public string? EndTime { get; set; }
        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender Is Required")]
        public Gender Gender { get; set; }
        [Required(ErrorMessage = "Phone Number Is Required")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Birth Date")]
        [Required(ErrorMessage = "Birth Date Is Required")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Profile Image")]
        public string? ProfileImg { get; set; }
        [NotMapped]
        [Display(Name = "Upload File")]
        public IFormFile? ImageFile { get; set; }
        [Display(Name = "Email"), StringLength(200)]
        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password Is Required"), StringLength(100)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords Not Match")]
        public string ConfirmPassword { get; set; }
    }
}
