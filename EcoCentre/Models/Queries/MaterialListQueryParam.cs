namespace EcoCentre.Models.Queries
{
	public class MaterialListQueryParam
	{
		public string Id { get; set; }
		public string Term { get; set; }
		public bool HasContainer { get; set; }
		public bool? OnlyCurrentHub { get; set; }
		public string Municipality { get; set; }
		public bool Active { get; set; }
	}
}