using System.Net.Mail;

namespace HearPrediction.Api.DTO
{
	public class MailRequestDto
	{
		public MailAddress To { get; set; }
		public string Subject { get; set; }
		public string Content { get; set; }
		public MailRequestDto(string to, string subject, string content)
		{
			To = new MailAddress(to);
			//To.AddRange(to.Select(x => new MailAddress(x)));
			subject = Subject;
			Content = content;
		}
	}
}
