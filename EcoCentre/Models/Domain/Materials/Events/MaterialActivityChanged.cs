namespace EcoCentre.Models.Domain.Materials.Events
{
	public class MaterialActivityChanged
	{
		public string MaterialId { get; set; }
		public bool IsActive { get; set; }
	}
}