using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "National ID")]
        public long SSN { get; set; }

        [Display(Name = "Insurance Number")]
        public int? Insurance_No { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public virtual ICollection<MedicalTest> MedicalTests { get; set; }
        public Patient()
        {
            Appointments = new Collection<Appointment>();
            Prescriptions = new Collection<Prescription>();
            MedicalTests = new Collection<MedicalTest>();
        }
    }
}
