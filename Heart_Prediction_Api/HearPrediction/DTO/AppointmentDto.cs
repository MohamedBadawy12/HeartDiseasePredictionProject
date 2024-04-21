using System;
using System.ComponentModel.DataAnnotations;

namespace HearPrediction.Api.DTO
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        [Display(Name = "First Name"), StringLength(100)]
        [Required(ErrorMessage = "First Name Is Required")]
        public string PateintFirstName { get; set; }
        [Display(Name = "Last Name"), StringLength(100)]
        [Required(ErrorMessage = "Last Name Is Required")]
        public string PatientLastName { get; set; }
        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender Is Required")]
        public Database.Enums.Gender PatientGender { get; set; }
        [Required(ErrorMessage = "Phone Number Is Required")]
        [Display(Name = "Phone Number")]
        public string PatientPhoneNumber { get; set; }
        [Display(Name = "Location"), StringLength(100)]
        public string? Location { get; set; }
        [Display(Name = "Name"), StringLength(150)]
        public string? DoctorName { get; set; }
        [Display(Name = "Price")]
        public string? Price { get; set; }
        [Display(Name = "Birth Date")]
        [Required(ErrorMessage = "Birth Date Is Required")]
        public DateTime PatientBirthDate { get; set; }
        [Display(Name = "Age")]
        public int PatientAge => CalculateAge();
        private int CalculateAge()
        {
            int age = DateTime.Now.Year - PatientBirthDate.Year;
            if (DateTime.Now.DayOfYear < PatientBirthDate.DayOfYear)
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
        public string PateintName { get; set; }
        public string DoctorEmail { get; set; }
        public string PatientEmail { get; set; }
        public string PatientImage { get; set; }
        public string PatientID { get; set; }
        public string ApDocotorId { get; set; }
        public long PatientSSN { get; set; }
        public int DoctorId { get; set; }
    }
}
