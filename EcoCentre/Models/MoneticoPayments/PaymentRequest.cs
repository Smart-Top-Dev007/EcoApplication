using System;

namespace EcoCentre.Models.MoneticoPayments
{
	public class PaymentRequest
	{
		public decimal Amount { get; set; }
		public string Reference { get; set; }
		public string FreeText { get; set; }
		public DateTime Date { get; set; }
		public string RedirectUrl { get; set; }
		public string RedirectSuccessUrl { get; set; }
		public string RedirectErrorUrl { get; set; }
		public string Email { get; set; }
	}
}