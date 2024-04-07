using HearPrediction.Api.DTO;

namespace HearPrediction.Api.Interfaces
{
	public interface IMailService
	{
		void SendEmail(MailRequestDto mailRequestDto);
	}
}
