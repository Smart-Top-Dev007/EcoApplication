using System;
using System.Configuration;

namespace EcoCentre.Models.Infrastructure
{
	public static class LocalDateTime
	{
		private static readonly TimeZoneInfo Tzi;

		static LocalDateTime()
		{
			Tzi = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings.Get("TimeZone"));
		}

		public static DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Tzi);

		public static DateTime ToLocalDateTime(this DateTime date)
		{
			return TimeZoneInfo.ConvertTimeFromUtc(date, Tzi);
		}
	}
}