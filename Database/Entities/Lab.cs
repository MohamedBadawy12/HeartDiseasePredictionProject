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
        public string PhoneNumber { get; set; }
        [Display(Name = "Location")]
        public string Location { get; set; }
        [Display(Name = "Price")]
        public string Price { get; set; }
        [Display(Name = "Lab Image")]
        public string? LabImage { get; set; }
        [Display(Name = "Zone")]
        public string? Zone { get; set; }
        [Display(Name = "Start Time")]
        public string? StartTime { get; set; }
        [Display(Name = "End Time")]
        public string? EndTime { get; set; }
        [NotMapped]
        [Display(Name = "Upload File")]
        public IFormFile? ImageFile { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<MedicalAnalyst> MedicalAnalysts { get; set; }
        public Lab()
        {
            MedicalAnalysts = new Collection<MedicalAnalyst>();
        }
    }
}
