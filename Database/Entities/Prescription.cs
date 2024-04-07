using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
	public class Prescription
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string MedicineName { get; set; }
		public DateTime date { get; set; }
		public string DoctorEmail { get; set; }
		public string PatientEmail { get; set; }
		public string ApDoctorId { get; set; }
		[ForeignKey(nameof(ApDoctorId))]
		public ApplicationUser Doctorr { get; set; }
		public long PatientSSN { get; set; }
		[ForeignKey(nameof(PatientSSN))]
		public Patient Patient { get; set; }
		public int? DoctorId { get; set; }
		[ForeignKey(nameof(DoctorId))]
		public virtual Doctor? Doctor { get; set; }
		public Prescription()
		{
			date = DateTime.Now;
		}
	}
}
