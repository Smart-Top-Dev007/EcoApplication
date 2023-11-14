using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcoCitizenImport.Import;
using NLog;

namespace EcoCitizenImport.Tools
{
	public static class DataExtensions
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
			TValue defaultValue = default)
		{
			if (key == null) return defaultValue;
			return dict.TryGetValue(key, out var value) ? value : defaultValue;
		}

		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey[] keys,
			TValue defaultValue = default)
		{
			if (keys == null || keys.Length == 0) return defaultValue;
			TValue value = defaultValue;
			return keys.Any(k => dict.TryGetValue(k, out value)) ? value : defaultValue;
		}

		internal static T GetColumnValue<T>(this WorksheetDataRow row, string[] propertyNames, T defaultValue = default)
		{
			var obj = row?.GetValueOrDefault(propertyNames)?.Value;
			Logger.Trace($"Got value {obj} for {string.Join(", ", propertyNames)}");
			if (obj == null) return defaultValue;
			if (string.IsNullOrWhiteSpace(obj.ToString())) return defaultValue;
			if (typeof(T).IsAssignableFrom(typeof(DateTime)))
			{
				if (obj is double d)
				{
					return (T)(object)DateTime.FromOADate(d);
				}
			}
			var baseType = Nullable.GetUnderlyingType(typeof(T));
			if (baseType != null)
			{
				var value = Convert.ChangeType(obj, baseType);
				return (T) value;
			}
			return (T) Convert.ChangeType(obj, typeof(T));
		}

		internal static string GetStringColumnValue(this WorksheetDataRow row, string[] propertyNames)
		{
			return row.GetColumnValue<string>(propertyNames);
		}

		internal static DateTime? GetDateTimeColumnValue(this WorksheetDataRow row, string[] propertyNames)
		{
			return row.GetColumnValue<DateTime?>(propertyNames);
		}

		internal static int? GetIntColumnValue(this WorksheetDataRow row, string[] propertyNames)
		{
			return row.GetColumnValue<int?>(propertyNames);
		}
	}
}
