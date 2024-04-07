using Database.Entities;
using System.Collections.Generic;

namespace HearPrediction.Api.DTO
{
	public class UserDetailDTO
	{
		public Patient Patient { get; set; }
		public IEnumerable<Appointment> Appointments { get; set; }
		public int CountAppointments { get; set; }
	}
}
