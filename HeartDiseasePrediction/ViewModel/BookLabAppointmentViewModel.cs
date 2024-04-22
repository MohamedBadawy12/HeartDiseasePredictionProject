using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeartDiseasePrediction.ViewModel
{
    public class BookLabAppointmentViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Phone Number")]
        public string LabPhoneNumber { get; set; }
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
        [Display(Name = "Lab Image")]
        public string? LabImage { get; set; }
        [NotMapped]
        [Display(Name = "Upload File")]
        public IFormFile? ImageFile { get; set; }
        [Required, Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Required, Display(Name = "Time")]
        public string Time { get; set; }
        //public string PatientPhone { get; set; }
        //public string PateintName { get; set; }
        public string LabEmail { get; set; }
        public string PatientEmail { get; set; }
        public string PatientID { get; set; }
        //public long PatientSSN { get; set; }
        public int LabId { get; set; }
    }
}
