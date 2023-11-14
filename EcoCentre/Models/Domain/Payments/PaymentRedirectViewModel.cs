using System.Collections.Generic;

namespace EcoCentre.Models.Domain.Payments
{
	public class PaymentRedirectViewModel
	{
		public Dictionary<string, string> Values { get; set; }
		public string RedirectUrl { get; set; }
	}
}