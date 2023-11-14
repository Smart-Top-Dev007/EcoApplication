namespace EcoCentre.Models.MoneticoPayments
{
	public class PaymentResult
	{
		public PaymentStatus PaymentStatus { get; set; }
		public string Pareq { get; set; }
		public string Md { get; set; }
		public string Urlacs { get; set; }
		public string ErrorDescription { get; set; }
		public string ErrorCode { get; set; }
	}

	public enum PaymentStatus
	{
		Declined = 1,
		Approved = 2,
		ApprovedTest = 3
	}
}