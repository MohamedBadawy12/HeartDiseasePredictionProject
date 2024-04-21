using System;
using System.ComponentModel.DataAnnotations;

namespace HearPrediction.Api.DTO
{
    public class EditAppointmentDto
    {
        [Required, Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Required, Display(Name = "Time")]
        public string Time { get; set; }
        public string PatientPhone { get; set; }
        public string PateintName { get; set; }
        public string PatinetImage { get; set; }
        //public int DoctorId { get; set; }
        public long PatientSSN { get; set; }
    }
}
