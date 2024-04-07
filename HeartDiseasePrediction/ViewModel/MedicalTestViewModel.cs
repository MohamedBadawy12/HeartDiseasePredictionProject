using Database.Enums;
using System.ComponentModel.DataAnnotations;

namespace HeartDiseasePrediction.ViewModel
{
    public class MedicalTestViewModel
    {
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
    }
}
