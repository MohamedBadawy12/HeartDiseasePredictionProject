using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeartDiseasePrediction.ViewModel
{
    public class LabViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Location Is Required")]
        [Display(Name = "Location")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Price Is Required")]
        [Display(Name = "Price")]
        public string Price { get; set; }
        [Required(ErrorMessage = "Phone Number Is Required")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email"), StringLength(200)]
        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Profile Image")]
        public string? ProfileImg { get; set; }
        [NotMapped]
        [Display(Name = "Upload File")]
        public IFormFile? ImageFile { get; set; }
    }
}
