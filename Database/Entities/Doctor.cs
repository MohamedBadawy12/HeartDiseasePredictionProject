using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Location")]
        public string? Location { get; set; }
        [Display(Name = "Name")]
        public string? Name { get; set; }
        [Display(Name = "Price")]
        public string? Price { get; set; }
        public bool IsAvailable { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Prescription> prescriptions { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        //public virtual ICollection<MedicalTest> medicalTests { get; set; }
        //public virtual ICollection<Prediction> Predictions { get; set; }
        public Doctor()
        {
            Appointments = new Collection<Appointment>();
            prescriptions = new Collection<Prescription>();
            Patients = new Collection<Patient>();
            //medicalTests = new Collection<MedicalTest>();
            //Predictions = new Collection<Prediction>();
        }
    }
}
