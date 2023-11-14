using EcoCentre.Models.Domain;

namespace EcoCentre.Models.Infrastructure.SystemSettings
{
	/// <summary>
	/// Represents settings that are not configurable by application users (including admin).
	/// These are set during application set up.
	/// </summary>
	public class SystemSettings : Entity
	{
		public bool IsObnlEnabled { get; set; }
	}
}