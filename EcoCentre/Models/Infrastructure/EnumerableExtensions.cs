using System.Collections.Generic;
using System.Linq;

namespace EcoCentre.Models.Infrastructure
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return Enumerable.Empty<T>();
			}
			return enumerable;
		}

		public static string JoinBy<T>(this IEnumerable<T> enumerable, string separator)
		{
			if (enumerable == null)
			{
				return string.Empty;
			}

			return string.Join(separator, enumerable);
		}
	}
}