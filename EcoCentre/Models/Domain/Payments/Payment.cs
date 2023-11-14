using System;

namespace EcoCentre.Models.Domain.Payments
{
	public class Payment
	{
		public PaymentMethod PaymentMethod { get; set; }
		public UserInfo ProcessedByUser { get; set; }
		public DateTime DateProcessed { get; set; }
		public bool IsTestPayment { get; set; }
		public DateTime MoneticoPaymentDate { get; set; }
		public decimal Amount { get; set; }
		public string Reference { get; set; }
	}
}