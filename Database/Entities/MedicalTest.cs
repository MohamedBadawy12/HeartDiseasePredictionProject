using Database.Enums;
using Microsoft.ML.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
	public class MedicalTest
	{
		[Key]
		[LoadColumn(1)]
		public int Id { get; set; }
		[Required, Display(Name = "Age")]
		[LoadColumn(2)]
		public float Age { get; set; }
		[Required, Display(Name = "Gender")]
		[LoadColumn(3)]
		public Gender Gender { get; set; }
		[Required, Display(Name = "Smoking")]
		[LoadColumn(4)]
		public float Smoking { get; set; }
		[Required, Display(Name = "Number Of Cigarettes")]
		[LoadColumn(5)]
		public float NumberOfCigarettes { get; set; }
		[Required, Display(Name = "Blood Pressure Medicine")]
		[LoadColumn(6)]
		public float BloodPressureMedicine { get; set; }
		[Required, Display(Name = "Prevalent Stroke")]
		[LoadColumn(7)]
		public float PrevalentStroke { get; set; }
		[Required, Display(Name = "Prevalent hypertension")]
		[LoadColumn(8)]
		public float Prevalenthypertension { get; set; }
		[Required, Display(Name = "Diabetes")]
		[LoadColumn(9)]
		public float Diabetes { get; set; }
		[Required, Display(Name = "Cholesterol Level")]
		[LoadColumn(10)]
		public float CholesterolLevel { get; set; }
		[Required, Display(Name = "Systolic Blood Pressure")]
		[LoadColumn(11)]
		public float SystolicBloodPressure { get; set; }
		[Required, Display(Name = "Diastolic Blood Pressure")]
		[LoadColumn(12)]
		public float DiastolicBloodPressure { get; set; }
		[Required, Display(Name = "BMI")]
		[LoadColumn(13)]
		public float BMI { get; set; }
		[Required, Display(Name = "Glucose Level")]
		[LoadColumn(14)]
		public float GlucoseLevel { get; set; }
		//[LoadColumn(15)]
		//public bool Label { get; set; }
		public string UserId { get; set; }
		[ForeignKey(nameof(UserId))]
		public ApplicationUser MedicalAnalayst { get; set; }
		public long PatientSSN { get; set; }
		[ForeignKey(nameof(PatientSSN))]
		public Patient patient { get; set; }
		public int MedicalAnalystId { get; set; }
		[ForeignKey(nameof(MedicalAnalystId))]
		public MedicalAnalyst MedicalAnalystt { get; set; }
		public DateTime date { get; set; }
		public MedicalTest()
		{
			date = DateTime.Now;
		}
	}
}
