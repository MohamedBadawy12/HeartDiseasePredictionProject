using System;
using System.ComponentModel.DataAnnotations;

namespace HeartDiseasePrediction.ViewModel
{
    public class LabAppointmentViewModel
    {
        public int Id { get; set; }
        [Display(Name = "First Name"), StringLength(100)]
        [Required(ErrorMessage = "First Name Is Required")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name"), StringLength(100)]
        [Required(ErrorMessage = "Last Name Is Required")]
        public string LastName { get; set; }
        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender Is Required")]
        public Database.Enums.Gender Gender { get; set; }
        [Required(ErrorMessage = "Phone Number Is Required")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Location"), StringLength(100)]
        public string? Location { get; set; }
        [Display(Name = "Name"), StringLength(150)]
        public string? Name { get; set; }
        [Display(Name = "Price")]
        public string? Price { get; set; }
        [Display(Name = "Birth Date")]
        [Required(ErrorMessage = "Birth Date Is Required")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Email"), StringLength(200)]
        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; }
        [Display(Name = "Patient Image")]
        public string PatientImage { get; set; }
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
        public string PatientPhone { get; set; }
        public string LabPhoneNumber { get; set; }
        public string PateintName { get; set; }
        public string PatientEmail { get; set; }
        public string LabEmail { get; set; }
        public string PatientID { get; set; }
        public long PatientSSN { get; set; }
        public int LabId { get; set; }
    }
}
