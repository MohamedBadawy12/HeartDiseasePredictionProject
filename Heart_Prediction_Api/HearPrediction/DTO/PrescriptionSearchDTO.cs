using System;

namespace HearPrediction.Api.DTO
{
	public class PrescriptionSearchDTO
	{
		public string MedicienName { get; set; }
		public DateTime Date { get; set; }
		public long patientSSN { get; set; }
	}
}
