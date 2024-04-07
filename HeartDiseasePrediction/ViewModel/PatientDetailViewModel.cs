using Database.Entities;
using System.Collections.Generic;

namespace HeartDiseasePrediction.ViewModel
{
    public class PatientDetailViewModel
    {
        public Patient Patient { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
        public int CountAppointments { get; set; }
        public int CountAttendance { get; set; }
    }
}
