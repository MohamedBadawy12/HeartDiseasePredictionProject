﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Database.Entities
{
	public class AuthModel
	{
		public string Message { get; set; }
		public bool IsAuthenticated { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public List<string> Roles { get; set; }
		public string Token { get; set; }
		public DateTime ExpireOn { get; set; }
		[JsonIgnore]
		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenExpiration { get; set; }
	}
}
