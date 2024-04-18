using Microsoft.Extensions.Configuration;
using Repositories.ViewModel;
using Services.Settings.MailSettings;
using System.Net;
using System.Net.Mail;

namespace Repositories
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IConfiguration _configuration;
        public MailService(MailSettings mailSettings, IConfiguration configuration)
        {
            _configuration = configuration;
            _mailSettings = mailSettings;
        }
        public void SendEmail(MailRequestViewModel mailRequestViewModel)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("hospitalheart21@gmail.com"),
                Subject = mailRequestViewModel.Subject,
                Body = mailRequestViewModel.Content,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(new MailAddress(mailRequestViewModel.To.ToString()));
            send(mailMessage);
        }
        private void send(MailMessage mailMessage)
        {
            try
            {
                var smtpClient = new System.Net.Mail.SmtpClient(_mailSettings.SmtpServer)
                {
                    Port = _mailSettings.Port,//in app setting
                    Credentials = new NetworkCredential(_mailSettings.UserName,
                    _mailSettings.Password),//in app setting
                    EnableSsl = true,//in app setting
                };


                smtpClient.Send(mailMessage.From.ToString(), mailMessage.To.ToString(), mailMessage.Subject, mailMessage.Body);

                mailMessage.To.Add(mailMessage.To.ToString());

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to send email.", ex);
            }
            finally
            {
                //client.Disconnect(true);
                //client.Dispose();
            }
        }
    }
}
