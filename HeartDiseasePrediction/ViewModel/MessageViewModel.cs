using System;

namespace HeartDiseasePrediction.ViewModel
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string Messages { get; set; }
        public DateTime Date { get; set; }
        public string PatientEmail { get; set; }
        public string DoctorEmail { get; set; }
        public string DoctorId { get; set; }
        public MessageViewModel()
        {
            Date = DateTime.Now;
        }
    }
}
