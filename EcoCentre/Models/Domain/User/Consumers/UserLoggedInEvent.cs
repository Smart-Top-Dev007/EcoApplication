namespace EcoCentre.Models.Domain.User.Consumers
{
	public class UserLoggedInEvent
	{
		public string UserId { get; set; }
		public string HubId { get; set; }
	}
}