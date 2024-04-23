using Database.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    public class MedicalTest
    {
        [Key]
        public int Id { get; set; }
        [Required, Display(Name = "Age")]
        public float Age { get; set; }
        [Required, Display(Name = "Gender")]
        public Gender Gender { get; set; }
        [Required, Display(Name = "Smoking")]
        public float Smoking { get; set; }
        [Required, Display(Name = "Number Of Cigarettes")]
        public float NumberOfCigarettes { get; set; }
        [Required, Display(Name = "Blood Pressure Medicine")]
        public float BloodPressureMedicine { get; set; }
        [Required, Display(Name = "Prevalent Stroke")]
        public float PrevalentStroke { get; set; }
        [Required, Display(Name = "Prevalent hypertension")]
        public float Prevalenthypertension { get; set; }
        [Required, Display(Name = "Diabetes")]
        public float Diabetes { get; set; }
        [Required, Display(Name = "Cholesterol Level")]
        public float CholesterolLevel { get; set; }
        [Required, Display(Name = "Systolic Blood Pressure")]
        public float SystolicBloodPressure { get; set; }
        [Required, Display(Name = "Diastolic Blood Pressure")]
        public float DiastolicBloodPressure { get; set; }
        [Required, Display(Name = "BMI")]
        public float BMI { get; set; }
        [Required, Display(Name = "Glucose Level")]
        public float GlucoseLevel { get; set; }
        [Required, Display(Name = "Glucose Level")]
        public float HeartRate { get; set; }
        [Display(Name = "Prediction")]
        public float? Prediction { get; set; }
        [Display(Name = "Probability")]
        public float? Probability { get; set; }
        //[LoadColumn(15)]
        //public bool Label { get; set; }
        //public int PredictionId { get; set; }
        //[ForeignKey(nameof(PredictionId))]
        //public Prediction Prediction { get; set; }
        [Required, Display(Name = "Patient Name")]
        public string PatientName { get; set; }
        [Required, Display(Name = "Patient Email")]
        public string PatientEmail { get; set; }
        [Required, Display(Name = "Lab Email")]
        public string LabEmail { get; set; }
        [Required, Display(Name = "Medical Analyst Name")]
        public string MedicalAnalystName { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser Labb { get; set; }
        public long PatientSSN { get; set; }
        [ForeignKey(nameof(PatientSSN))]
        public Patient Patient { get; set; }
        public int? LabId { get; set; }
        [ForeignKey(nameof(LabId))]
        public Lab? Lab { get; set; }
        //public int? MedicalAnalystId { get; set; }
        //[ForeignKey(nameof(MedicalAnalystId))]
        //public MedicalAnalyst? MedicalAnalystt { get; set; }
        public DateTime Date { get; set; }
        public MedicalTest()
        {
            Date = DateTime.Now;
        }
    }
}
