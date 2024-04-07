using System;
using System.ComponentModel.DataAnnotations;

namespace HearPrediction.Api.DTO
{
	public class AppointmentFormDto
	{
		public int id { get; set; }
		[Required]
		public DateTime Date { get; set; }
		//public  string Time { get; set; }
		[Required]
		public string StartTime { get; set; }
		[Required]
		public string EndTime { get; set; }
		[Required]
		public string Detail { get; set; }
		[Required]
		public bool Status { get; set; }
		public long? patientSSN { get; set; }
		//public IEnumerable<Patient> Patients { get; set; }
		public int doctorId { get; set; }
		//public IEnumerable<Doctor> Doctors { get; set; }
		//public string Heading { get; set; }

		//public IEnumerable<Appointment> Appointments { get; set; }
		//public DateTime GetStartDateTime()
		//{
		//	return DateTime.Parse(string.Format("{0} {1}", Date, Time));
		//}
	}
}
