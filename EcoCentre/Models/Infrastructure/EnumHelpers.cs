using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace EcoCentre.Models.Infrastructure
{
	public static class EnumHelpers
	{
		public static string GetDescription(object enumValue)
		{
			if (enumValue == null)
			{
				return null;
			}
			var attribute = enumValue
				.GetType()
				.GetField(enumValue.ToString())
				.GetCustomAttributes(typeof(DisplayAttribute), false)
				.FirstOrDefault() as DisplayAttribute;

			if (attribute == null)
			{
				return enumValue.ToString();
			}
			var type = attribute.ResourceType;
			if (type == null)
			{
				return attribute.Name;
			}
			var property = type.GetProperty(attribute.Name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
			return property.GetValue(null, null).ToString();
		}

		public static string GetDescription(this Enum enumValue)
		{
			return GetDescription(enumValue as object);
		}

	}
}