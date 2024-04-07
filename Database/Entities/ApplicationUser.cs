using Database.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "National ID")]
        public long? SSN { get; set; }

        [Display(Name = "Insurance Number")]
        public int? Insurance_No { get; set; }
        [Display(Name = "Age")]
        public int Age => CalculateAge();
        [Display(Name = "Gender")]
        public Gender Gender { get; set; }
        [Display(Name = "Profile Image")]
        public string? ProfileImg { get; set; }
        [NotMapped]
        [Display(Name = "Upload File")]
        public IFormFile? ImageFile { get; set; }
        [Display(Name = "Location")]
        public string? Location { get; set; }
        [Display(Name = "Name")]
        public string? Name { get; set; }
        [Display(Name = "Price")]
        public string? Price { get; set; }
        private int CalculateAge()
        {
            int age = DateTime.Now.Year - BirthDate.Year;
            if (DateTime.Now.DayOfYear < BirthDate.DayOfYear)
            {
                age--;
            }
            return age;
        }
        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
