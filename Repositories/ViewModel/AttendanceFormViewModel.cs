using System.ComponentModel.DataAnnotations;

namespace Repositories.ViewModel
{
    public class AttendanceFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public string HospitalRemarks { get; set; }

        [Required]
        [StringLength(255)]
        public string Diagnosis { get; set; }

        public string SecondDiagnosis { get; set; }
        public string ThirdDiagnosis { get; set; }

        [Required]
        public string Therapy { get; set; }


        public DateTime Date { get; set; }

        public string Heading { get; set; }

        [Required]
        public long Patient { get; set; }


        [Required]
        public int Doctor { get; set; }
    }
}
