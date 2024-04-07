using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    public class MedicalAnalyst
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
        public int LabId { get; set; }
        [ForeignKey(nameof(LabId))]
        public virtual Lab Lab { get; set; }
        public virtual ICollection<MedicalTest> medicalTests { get; set; }
        public MedicalAnalyst()
        {
            medicalTests = new Collection<MedicalTest>();
        }
    }
}
