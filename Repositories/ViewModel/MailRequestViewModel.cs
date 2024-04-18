using System.Net.Mail;

namespace Repositories.ViewModel
{
    public class MailRequestViewModel
    {
        public MailAddress To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public MailRequestViewModel(string to, string subject, string content)
        {
            To = new MailAddress(to);
            //To.AddRange(to.Select(x => new MailAddress(x)));
            subject = Subject;
            Content = content;
        }
    }
}
