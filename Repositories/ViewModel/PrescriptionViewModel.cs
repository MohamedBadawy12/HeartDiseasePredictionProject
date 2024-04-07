using System.ComponentModel.DataAnnotations;

namespace Repositories.ViewModel
{
	public class PrescriptionViewModel
	{
		public int Id { get; set; }
		[Required, Display(Name = "Mecdicine Name")]
		public string MedicineName { get; set; }
		//public DateTime date { get; set; }
		[Display(Name = "Doctor Email")]
		public string DoctorEmail { get; set; }
		[Display(Name = "Patient Email")]
		public string PatientEmail { get; set; }
		public string ApDoctorId { get; set; }
		[Display(Name = "Patient SSN")]
		public long PatientSSN { get; set; }
		public int? DoctorId { get; set; }
		//public PrescriptionViewModel()
		//{
		//	date = DateTime.Now;
		//}
	}
}
