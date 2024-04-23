using Database.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HearPrediction.Api.DTO
{
    public class ProfileUpdateDto
    {
        //public string UserId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Zone")]
        public string Zone { get; set; }
        [Display(Name = "About")]
        public string About { get; set; }
        [Required]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        //[Display(Name = "National Id")]
        //public long? SSN { get; set; }
        //[Display(Name = "Insurance Number")]
        //public int? Insurance_No { get; set; }
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }
        [NotMapped]
        [Display(Name = "Upload File")]
        public IFormFile? ImageFile { get; set; }
        [Display(Name = "Profile Image")]
        public string? ProfileImg { get; set; }
        [Display(Name = "Location")]
        public string? Location { get; set; }
        [Display(Name = "Name")]
        public string? Name { get; set; }
        [Display(Name = "Price")]
        public string? Price { get; set; }
    }
}
