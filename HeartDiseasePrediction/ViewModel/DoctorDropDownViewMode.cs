using Database.Entities;
using System.Collections.Generic;

namespace HeartDiseasePrediction.ViewModel
{
    public class DoctorDropDownViewMode
    {
        public DoctorDropDownViewMode()
        {
            doctors = new List<Doctor>();
        }
        public List<Doctor> doctors { get; set; }
    }
}
