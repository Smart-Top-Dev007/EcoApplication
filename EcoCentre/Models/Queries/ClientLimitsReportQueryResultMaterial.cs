using EcoCentre.Models.Domain.Materials;

namespace EcoCentre.Models.Queries
{
	public class ClientLimitsReportQueryResultMaterial
	{
		public Material Material { get; set; }
		public decimal MaxQuantity { get; set; }
		public decimal QuantitySoFar { get; set; }
	}
}