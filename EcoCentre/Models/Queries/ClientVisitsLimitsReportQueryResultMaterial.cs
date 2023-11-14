using EcoCentre.Models.Domain.Materials;

namespace EcoCentre.Models.Queries
{
	public class ClientVisitsLimitsReportQueryResultMaterial
	{
		public Material Material { get; set; }
		public decimal QuantitySoFar { get; set; }

        public bool IsExcluded { get; set; }
	}
}