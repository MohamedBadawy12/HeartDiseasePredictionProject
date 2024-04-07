using Database.Entities;
using System.Collections.Generic;

namespace HearPrediction.Api.DTO
{
	public class AppointmentsDropDownDTO
	{
		public List<Appointment> Appointments { get; set; }
		public AppointmentsDropDownDTO()
		{
			Appointments = new List<Appointment>();
		}
	}
}
