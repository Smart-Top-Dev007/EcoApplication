using System.Collections.Generic;
using EcoCentre.Models.Domain.Hubs;

namespace EcoCentre.Models.Domain.Clients.Commands
{
	public class LoginPageViewModel
	{
		public List<Hub> Hubs { get; set; }
		public string HubId { get; set; }
	}
}