using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeartDiseasePrediction.ViewModel
{
    public class EditAccountProfile
    {
        public string UserId { get; set; }
        [Display(Name = "First Name"), StringLength(100)]
        public string FirstName { get; set; }
        [Display(Name = "Last Name"), StringLength(100)]
        public string LastName { get; set; }
        [Display(Name = "Gender")]
        public Database.Enums.Gender Gender { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Email"), StringLength(200)]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Profile Image")]
        public string? ProfileImg { get; set; }
        [NotMapped]
        [Display(Name = "Upload File")]
        public IFormFile? ImageFile { get; set; }
        [Display(Name = "Age")]
        public int Age => CalculateAge();
        private int CalculateAge()
        {
            int age = DateTime.Now.Year - BirthDate.Year;
            if (DateTime.Now.DayOfYear < BirthDate.DayOfYear)
            {
                age--;
            }
            return age;
        }
        [TempData]
        public string StatusMessage { get; set; }
        [Display(Name = "SSN")]
        public long SSN { get; set; }
        [Display(Name = "Insurance Number")]
        public int Insurance_No { get; set; }
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
        [Display(Name = "Start Time"), StringLength(100)]
        public string? StartTime { get; set; }
        [Display(Name = "End Time"), StringLength(100)]
        public string? EndTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
