using Microsoft.ML.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    public class Prediction
    {
        [Key]
        public int Id { get; set; }
        public DateTime date { get; set; }
        [ColumnName("PredictedLabel")]
        public bool prediction;

        public float Probability;

        public float Score;
        //public long PatientId { get; set; }
        //[ForeignKey(nameof(PatientId))]
        //public Patient Patient { get; set; }
        public int MedicalId { get; set; }
        [ForeignKey(nameof(MedicalId))]
        public MedicalTest MedicalTest { get; set; }
        //public ICollection<MedicalTest> MedicalTests { get; set; }
        public Prediction()
        {
            date = DateTime.Now;
        }
    }
}
