using System.ComponentModel.DataAnnotations;

namespace HearPrediction.Api.DTO
{
    public class CreatePrescriptionDto
    {
        [Required, Display(Name = "Mecdicine Name")]
        public string MedicineName { get; set; }
    }
}
