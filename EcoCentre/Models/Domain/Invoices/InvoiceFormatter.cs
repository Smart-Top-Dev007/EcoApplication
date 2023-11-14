using EcoCentre.Models.Domain.Hubs;

namespace EcoCentre.Models.Domain.Invoices
{
	public class InvoiceFormatter
	{
		private readonly string _invoicePattern;
		private readonly Hub _hub;

		public InvoiceFormatter(string invoicePattern, Hub hub)
		{
			_invoicePattern = invoicePattern;
			_hub = hub;
		}
		
		public string FormatInvoiceNo(int year, int sequentialNo)
		{
			return _invoicePattern
				.Replace("{YEAR}", year.ToString())
				.Replace("{HUBID}", _hub?.InvoiceIdentifier)
				.Replace("{SEQNO}", sequentialNo.ToString("00000"));
		}
	}
}