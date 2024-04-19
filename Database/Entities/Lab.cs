using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    public class Lab
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Name")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Display(Name = "Phone Number")]
        public long PhoneNumber { get; set; }
        [Display(Name = "Location")]
        public string Location { get; set; }
        [Display(Name = "Price")]
        public string Price { get; set; }
        [Display(Name = "Lab Image")]
        public string? LabImage { get; set; }
        [NotMapped]
        [Display(Name = "Upload File")]
        public IFormFile? ImageFile { get; set; }
        //public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<MedicalAnalyst> MedicalAnalysts { get; set; }
        public Lab()
        {
            //Patients = new Collection<Patient>();
            MedicalAnalysts = new Collection<MedicalAnalyst>();
        }
    }
}
