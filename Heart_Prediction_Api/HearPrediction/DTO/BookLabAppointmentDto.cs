using System;
using System.ComponentModel.DataAnnotations;

namespace HearPrediction.Api.DTO
{
    public class BookLabAppointmentDto
    {
        //public int Id { get; set; }
        //[Display(Name = "Phone Number")]
        //public string LabPhoneNumber { get; set; }
        //[Display(Name = "Location"), StringLength(100)]
        //public string? Location { get; set; }
        //[Display(Name = "Name"), StringLength(150)]
        //public string? Name { get; set; }
        //[Display(Name = "Price")]
        //public string? Price { get; set; }
        //[Display(Name = "Lab Image")]
        //public string? LabImage { get; set; }
        //[NotMapped]
        //[Display(Name = "Upload File")]
        //public IFormFile? ImageFile { get; set; }
        //[Required, Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Required, Display(Name = "Time")]
        public string Time { get; set; }
        public string PatientPhone { get; set; }
        public string PateintName { get; set; }
        //public string LabEmail { get; set; }
        //public string PatientEmail { get; set; }
        //public string PatientID { get; set; }
        //public long PatientSSN { get; set; }
        //public int LabId { get; set; }
    }
}
