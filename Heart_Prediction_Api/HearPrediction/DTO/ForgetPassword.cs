using System.ComponentModel.DataAnnotations;

namespace HearPrediction.Api.DTO
{
	public class ForgetPassword
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}
}
