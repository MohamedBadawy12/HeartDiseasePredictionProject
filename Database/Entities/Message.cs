using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Messages { get; set; }
        public DateTime Date { get; set; }
        public string PatientEmail { get; set; }
        public string DoctorEmail { get; set; }
        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }
        public Message()
        {
            Date = DateTime.Now;
        }
    }
}
