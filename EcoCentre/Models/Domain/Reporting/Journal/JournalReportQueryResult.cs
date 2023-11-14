using EcoCentre.Models.Queries;

namespace EcoCentre.Models.Domain.Reporting.Journal
{
	public class JournalReportQueryResult
	{
		public PagedCollection<JournalReportResultRow> Report { get; set; }
		public JournalReportSummaryResult Summary { get; set; }
     
	}
}