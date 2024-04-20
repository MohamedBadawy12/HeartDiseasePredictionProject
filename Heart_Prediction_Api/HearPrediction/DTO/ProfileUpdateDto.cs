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
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        //[Display(Name = "National Id")]
        //public long? SSN { get; set; }
        //[Display(Name = "Insurance Number")]
        //public int? Insurance_No { get; set; }
        [Required]
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
