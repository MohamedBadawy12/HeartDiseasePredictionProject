using System;

namespace HearPrediction.Api.DTO
{
	public class SearchDto
	{
		public string Name { get; set; }
		public string FirstName { get; set; }
		public long SSN { get; set; }
		public string phoneNo { get; set; }
		public string Option { get; set; }
		public DateTime Date { get; set; }
	}
}
