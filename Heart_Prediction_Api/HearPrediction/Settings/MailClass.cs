using System.Collections.Generic;

namespace HearPrediction.Api.Settings
{
	public class MailClass
	{
		public string FromMail { get; set; } = "hospitalheart21@gmail.com";
		public string FromMailPassword { get; set; } = "H123456h";
		public List<string> ToMail { get; set; } = new List<string>();
		public string Subject { get; set; } = "";
		public string Body { get; set; } = "";
		public bool IsBodyHtml { get; set; } = true;
		public List<string> Attachments { get; set; } = new List<string>();

	}
}
