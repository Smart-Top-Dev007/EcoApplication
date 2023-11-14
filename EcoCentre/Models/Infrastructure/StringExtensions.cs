using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.Infrastructure
{
	public static class StringExtensions
	{
		public static bool IsNotEmpty(this string str)
		{
			return !string.IsNullOrEmpty(str);
		}
	}
}