using Database.Enums;
using System.ComponentModel.DataAnnotations;

namespace HeartDiseasePrediction.ViewModel
{
    public class EditMedicalTestViewModel
    {
        public int Id { get; set; }
        [Required, Display(Name = "Age")]
        public int Age { get; set; }
        [Required, Display(Name = "Gender")]
        public Gender Gender { get; set; }
        [Required, Display(Name = "Smoking")]
        public int Smoking { get; set; }
        [Required, Display(Name = "Number Of Cigarettes")]
        public int NumberOfCigarettes { get; set; }
        [Required, Display(Name = "Blood Pressure Medicine")]
        public int BloodPressureMedicine { get; set; }
        [Required, Display(Name = "Prevalent Stroke")]
        public int PrevalentStroke { get; set; }
        [Required, Display(Name = "Prevalent hypertension")]
        public int Prevalenthypertension { get; set; }
        [Required, Display(Name = "Diabetes")]
        public int Diabetes { get; set; }
        [Required, Display(Name = "Cholesterol Level")]
        public int CholesterolLevel { get; set; }
        [Required, Display(Name = "Systolic Blood Pressure")]
        public float SystolicBloodPressure { get; set; }
        [Required, Display(Name = "Diastolic Blood Pressure")]
        public float DiastolicBloodPressure { get; set; }
        [Required, Display(Name = "BMI")]
        public float BMI { get; set; }
        [Required, Display(Name = "Heart Rate")]
        public int HeartRate { get; set; }
        [Required, Display(Name = "Glucose Level")]
        public int GlucoseLevel { get; set; }
        public long PatientSSN { get; set; }
        public string PatientName { get; set; }
        public string MedicalAnalystName { get; set; }
        public string PatientEmail { get; set; }
        public string LabEmail { get; set; }
    }
}
