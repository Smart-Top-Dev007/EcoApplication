using System.Collections.Generic;

namespace EcoCentre.Models.Infrastructure
{
	public static class DictionaryExtensions
	{
		public static TValue Get<TKey,TValue>(this Dictionary<TKey, TValue> dict, TKey key)
		{
			if (dict.ContainsKey(key))
			{
				return dict[key];
			}

			return default(TValue); 
		}
	}
}