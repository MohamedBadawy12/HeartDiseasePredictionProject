using System;

namespace HearPrediction.Api.DTO
{
	public class MessageDto
	{
		public int Id { get; set; }
		public string Messages { get; set; }
		public DateTime Date { get; set; }
		public string PatientEmail { get; set; }
		public string DoctorEmail { get; set; }
		public string DoctorId { get; set; }
		public MessageDto()
		{
			Date = DateTime.Now;
		}
	}
}
