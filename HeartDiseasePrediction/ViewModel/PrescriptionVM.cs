using System;
using System.ComponentModel.DataAnnotations;

namespace HeartDiseasePrediction.ViewModel
{
	public class PrescriptionVM
	{
		public int Id { get; set; }
		[Required, Display(Name = "Mecdicine Name")]
		public string MedicineName { get; set; }
		public DateTime date { get; set; }
		[Display(Name = "Doctor Email")]
		public string DoctorEmail { get; set; }
		[Display(Name = "Patient Email")]
		public string PatientEmail { get; set; }
		public string PatientID { get; set; }
		public string ApDoctorId { get; set; }
		[Display(Name = "Patient SSN")]
		public long PatientSSN { get; set; }
		[Display(Name = "Patient First Name")]
		public string PatientFirstName { get; set; }
		[Display(Name = "Patient First Name")]
		public string PatientLastName { get; set; }
		public int? DoctorId { get; set; }
		[Display(Name = "DoctorName")]
		public string DoctorName { get; set; }
		public PrescriptionVM()
		{
			date = DateTime.Now;
		}
	}
}
