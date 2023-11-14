namespace EcoCentre.Models.Domain.Materials.Events
{
	public class MaterialLimitChangedEvent
	{
		public string MaterialId { get; set; }
		public decimal NewLimit { get; set; }
	}
}