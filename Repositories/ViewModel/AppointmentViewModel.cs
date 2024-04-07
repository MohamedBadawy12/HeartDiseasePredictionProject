using System.ComponentModel.DataAnnotations;

namespace Repositories.ViewModel
{
	public class AppointmentViewModel
	{
		public int Id { get; set; }
		[Required, Display(Name = "Date")]
		public DateTime date { get; set; }
		[Required, Display(Name = "Time")]
		public string Time { get; set; }
		public string PateintName { get; set; }
		public string DoctorEmail { get; set; }
		public string PatientEmail { get; set; }
		public string PatientID { get; set; }
		public string ApDocotorId { get; set; }
		public long PatientSSN { get; set; }
		public int DoctorId { get; set; }
	}
}
