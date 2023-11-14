using System.Collections.Generic;

namespace EcoCentre.Models.Domain.Materials.Commands
{
	public class MaterialViewModel
	{
		public string Tag { get; set; }
		public string Name { get; set; }
		public bool Active { get; set; }
		public decimal Price { get; set; }
		public string Unit { get; set; }
		public double Weight { get; set; }
		public decimal MaxYearlyAmount { get; set; }
		public bool IsExcluded { get; set; }
		public List<HubMaterialSettings> HubSettings { get; set; }
	}
}