using System;

namespace EcoCentre.Models.Domain.User.Consumers
{
	public class UserLoggedOutEvent
	{
		public string UserId { get; set; }
		public string HubId { get; set; }
		public DateTime? LogoutDateUtc { get; set; }
		public LogoutType LogoutType { get; set; }
	}

	public enum LogoutType
	{
		UserInitiated,
		SessionTimeout
	}
}