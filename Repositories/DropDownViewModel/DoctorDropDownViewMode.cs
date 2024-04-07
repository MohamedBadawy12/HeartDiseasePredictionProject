using Database.Entities;

namespace Repositories.DropDownViewModel
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
