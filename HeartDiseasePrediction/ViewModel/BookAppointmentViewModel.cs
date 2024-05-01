using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeartDiseasePrediction.ViewModel
{
    public class BookAppointmentViewModel
    {
        public int Id { get; set; }
        [Display(Name = "First Name"), StringLength(100)]
        public string FirstName { get; set; }
        [Display(Name = "Last Name"), StringLength(100)]
        public string LastName { get; set; }
        [Display(Name = "Gender")]
        public Database.Enums.Gender Gender { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Location"), StringLength(200)]
        public string? Location { get; set; }
        [Display(Name = "About")]
        public string? About { get; set; }
        [Display(Name = "Zone"), StringLength(100)]
        public string? Zone { get; set; }
        [Display(Name = "Name"), StringLength(150)]
        public string? Name { get; set; }
        [Display(Name = "Price")]
        public string? Price { get; set; }
        public bool IsAvailable { get; set; }
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Email"), StringLength(200)]
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
        [Required, Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Required, Display(Name = "Time")]
        public string Time { get; set; }
        //public string PatientPhone { get; set; }
        //public string PateintName { get; set; }
        public string DoctorEmail { get; set; }
        public string PatientEmail { get; set; }
        public string PatientID { get; set; }
        public string ApDocotorId { get; set; }
        //public long PatientSSN { get; set; }
        public int DoctorId { get; set; }
    }
}
