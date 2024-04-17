using HeartDiseasePrediction.ViewModel;

namespace HeartDiseasePrediction.Helper
{
	public interface IMailService
	{
		void SendEmail(MailRequestViewModel mailRequestViewModel);
	}
}
