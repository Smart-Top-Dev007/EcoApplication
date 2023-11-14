using System;
using System.Collections.Generic;
using System.Web;

namespace EcoCentre.Models.Infrastructure
{
	public static class ObjectExtensions
	{
		public static List<T> AsList<T>(this T obj)
		{
			if (obj == null)
			{
				return new List<T>();
			}

			return new List<T> {obj};
		}

		public static string FormatIfNotEmpty(this object obj, string format)
		{
			var str = obj?.ToString();
			if (!string.IsNullOrWhiteSpace(str))
			{
				return string.Format(format, str);
			}

			return str;
		}

	}
}