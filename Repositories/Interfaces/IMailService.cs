using Repositories.ViewModel;

namespace Repositories
{
    public interface IMailService
    {
        void SendEmail(MailRequestViewModel mailRequestViewModel);
    }
}
