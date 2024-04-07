using System.ComponentModel.DataAnnotations;

namespace HearPrediction.Api.DTO
{
	public class WelcomeRequest
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		public string Email { get; set; }
	}
}
