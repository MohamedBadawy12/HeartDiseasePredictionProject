using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeartDiseasePrediction.ViewModel
{
    public class RegisterLabVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Location Is Required")]
        [Display(Name = "Location")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Zone Is Required")]
        [Display(Name = "Zone")]
        public string Zone { get; set; }
        [Required(ErrorMessage = "Start Time Is Required")]
        [Display(Name = "Start Time")]
        public string StartTime { get; set; }
        [Required(ErrorMessage = "End Time Is Required")]
        [Display(Name = "End Time")]
        public string EndTime { get; set; }
        [Display(Name = "About")]
        public string? About { get; set; }
        [Required(ErrorMessage = "Price Is Required")]
        [Display(Name = "Price")]
        public string Price { get; set; }
        [Required(ErrorMessage = "Phone Number Is Required")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email"), StringLength(200)]
        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Email is already Exist")]
        public string Email { get; set; }
        [Display(Name = "Profile Image")]
        public string? ProfileImg { get; set; }
        [NotMapped]
        [Display(Name = "Upload File")]
        public IFormFile? ImageFile { get; set; }
        [Required, StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password Is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords Not Match")]
        public string ConfirmPassword { get; set; }
    }
}
