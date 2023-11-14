namespace EcoCentre.Models.MoneticoPayments
{
	public class PaymentFields
	{
		public string Version { get; set; }
		public string Tpe { get; set; }
		public string Date { get; set; }
		public string Amount { get; set; }
		public string Reference { get; set; }
		public string FreeText { get; set; }
		public string Language { get; set; }
		public string Company { get; set; }
		public string Email { get; set; }
		public string RedirectUrl { get; set; }
		public string RedirectErrorUrl { get; set; }
		public string RedirectSuccessUrl { get; set; }
	}

	public class PaymentRedirectFields
	{
		public string Version { get; set; }
		public string Tpe { get; set; }
		public string Date { get; set; }
		public string Amount { get; set; }
		public string Reference { get; set; }
		public string FreeText { get; set; }
		public string Language { get; set; }
		public string Company { get; set; }
		public string ReturnUrl { get; set; }
		public string ReturnUrlOk { get; set; }
		public string ReturnUrlError { get; set; }
		public string Email { get; set; }
	}
}