using System;
using System.ComponentModel.DataAnnotations;

namespace HearPrediction.Api.DTO
{
	public class PrescriptionFormDTO
	{
		public int Id { get; set; }
		[Required, Display(Name = "Mecdicine Name")]
		public string MedicineName { get; set; }
		public DateTime Date { get; set; }
		public string DoctorEmail { get; set; }
		[Display(Name = "Patient Email")]
		public string PatientEmail { get; set; }
		public string ApDoctorId { get; set; }
		[Display(Name = "Patient SSN")]
		public long PatientSSN { get; set; }
		public int? DoctorId { get; set; }
		public string DoctorName { get; set; }

		public PrescriptionFormDTO()
		{
			Date = DateTime.Now;
		}
	}
}
