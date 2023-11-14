using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcoCentre.Models.Infrastructure
{
	public class DecimalModelBinder : DefaultModelBinder
	{
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

			if (valueProviderResult == null)
			{
				return base.BindModel(controllerContext, bindingContext);
			}

			decimal.TryParse(valueProviderResult.AttemptedValue.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var result);
			return result;
		}
	}
}