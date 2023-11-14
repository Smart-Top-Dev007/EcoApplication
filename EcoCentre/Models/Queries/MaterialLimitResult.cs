using System.Collections.Generic;

namespace EcoCentre.Models.Queries
{
	public class MaterialLimitResult
	{
		public List<MaterialLimitResultRow> Limits { get; set; }
		public List<MaterialLimitResultRow> AllLimits { get; set; }
	}
}