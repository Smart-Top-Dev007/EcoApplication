namespace EcoCentre.Models.Queries
{
	public class ClientLimitsReportQueryParams
	{
		public string ClientId { get; set; }
		public int Page { get; set; }
		public string Id { get; set; }
        public int? PageSize { get; set; }
	}
}