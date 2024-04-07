using Database.Entities;
using System.Collections.Generic;

namespace HearPrediction.Api.DTO
{
	public class DoctorDetailsDTO
	{
		public Doctor Doctor { get; set; }
		public IEnumerable<Appointment> UpcomingAppointments { get; set; }
		public IEnumerable<Appointment> Appointments { get; set; }
	}
}
