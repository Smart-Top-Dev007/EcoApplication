using EcoCentre.Models.Domain.Materials;

namespace EcoCentre.Models.Queries
{
	public class MaterialLimitResultRow
	{
		public MaterialLimitResultRow(Material material)
		{
			Material = material;
			Quantity = 0;
		    IsExcluded = material.IsExcluded;
		}
		public Material Material { get; set; }
		public decimal Quantity { get; set; }
        public bool IsExcluded { get; set; }
	}
}