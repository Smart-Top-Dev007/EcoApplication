using System.Collections.Generic;
using System.Linq;

namespace EcoCentre.Models.Domain.Reporting.Journal
{
	public class JournalReportSummaryResult
	{
		public int InvoiceCount { get; set; }
		public int UniqueAddressCount { get; set; }
		public decimal TotalAmountIncludingTaxes { get; set; }

		public JournalReportSummaryResult(List<InvoiceJournal> items)
		{
			//get unique invoice

			var invoicesummary = from p in items
								 group p by p.InvoiceNo
								 into grp
								 select new
								 {
									 Count = 1
								 };

			InvoiceCount = invoicesummary.Sum(x => x.Count);

			var addresssummary = from p in items
								 group p by new { p.CivicNumber, p.AptNumber, p.Street }
								 into grp
								 select new
								 {
									 Count = 1
								 };

			UniqueAddressCount = addresssummary.Sum(x => x.Count);

			TotalAmountIncludingTaxes = items.Sum(x => x.AmountIncludingTaxes);
		}
	}
}