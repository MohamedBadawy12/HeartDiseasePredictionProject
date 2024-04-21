using System;
using System.ComponentModel.DataAnnotations;

namespace HeartDiseasePrediction.ViewModel
{
    public class EditAppointmentViewModel
    {
        [Required, Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Required, Display(Name = "Time")]
        public string Time { get; set; }
        public string PatientPhone { get; set; }
        public string PateintName { get; set; }
        public string PatientImage { get; set; }
        //public int DoctorId { get; set; }
        //public long PatientSSN { get; set; }
    }
}
