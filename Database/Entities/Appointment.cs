using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        [Required, Display(Name = "Date")]
        public DateTime date { get; set; }
        [Required, Display(Name = "Time")]
        public string Time { get; set; }
        public string PateintName { get; set; }
        public string DoctorEmail { get; set; }
        public string PatientEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string PatientID { get; set; }
        public string PatientImage { get; set; }
        public bool IsAccepted { get; set; }
        [ForeignKey(nameof(PatientID))]
        public ApplicationUser Patientt { get; set; }
        public long PatientSSN { get; set; }
        [ForeignKey(nameof(PatientSSN))]
        public virtual Patient Patient { get; set; }
        public int DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public virtual Doctor Doctor { get; set; }
    }
}
