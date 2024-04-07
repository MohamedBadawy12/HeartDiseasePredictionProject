using System;
using System.ComponentModel.DataAnnotations;

namespace HearPrediction.Api.DTO
{
	public class PrescriptionDto
	{
		[Required, Display(Name = "Mecdicine Name")]
		public string MedicineName { get; set; }
		public DateTime date { get; set; }
		[Display(Name = "Patient Email")]
		public string PatientEmail { get; set; }
		[Display(Name = "Patient SSN")]
		public long PatientSSN { get; set; }
		public int DoctorId { get; set; }
		public PrescriptionDto()
		{
			date = DateTime.Now;
		}
	}
}
