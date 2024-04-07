using HearPrediction.Api.DTO;
using HearPrediction.Api.Interfaces;
using Microsoft.Extensions.Configuration;
using Services.Settings.MailSettings;
using System;
using System.Net;
using System.Net.Mail;

namespace Services
{
	public class MailService : IMailService
	{
		private readonly MailSettings _mailSettings;
		private readonly IConfiguration _configuration;

		public MailService(IConfiguration configuration)
		{
			_configuration = configuration;
			_mailSettings = _configuration.GetSection("MailSettings").Get<MailSettings>();
		}

		public void SendEmail(MailRequestDto mailRequestDto)
		{
			var mailMessage = new MailMessage
			{
				From = new MailAddress("hospitalheart21@gmail.com"),
				Subject = mailRequestDto.Subject,
				Body = mailRequestDto.Content,
				IsBodyHtml = true,
			};

			mailMessage.To.Add(new MailAddress(mailRequestDto.To.ToString()));
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
		//public async Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null)
		//{
		//    var email = new MimeMessage
		//    {
		//        Sender = MailboxAddress.Parse(_mailSettings.Email),
		//        Subject = subject
		//    };

		//    email.To.Add(MailboxAddress.Parse(mailTo));

		//    var builder = new BodyBuilder();

		//    if (attachments != null)
		//    {
		//        byte[] fileBytes;
		//        foreach (var file in attachments)
		//        {
		//            if (file.Length > 0)
		//            {
		//                using var ms = new MemoryStream();
		//                file.CopyTo(ms);
		//                fileBytes = ms.ToArray();

		//                builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
		//            }
		//        }
		//    }

		//    builder.HtmlBody = body;
		//    email.Body = builder.ToMessageBody();
		//    email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

		//    using var smtp = new SmtpClient();
		//    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
		//    smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
		//    await smtp.SendAsync(email);

		//    smtp.Disconnect(true);
		//}
	}
}
