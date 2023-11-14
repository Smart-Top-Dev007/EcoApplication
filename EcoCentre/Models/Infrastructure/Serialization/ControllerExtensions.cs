using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.Domain.Materials;

namespace EcoCentre.Models.Infrastructure.Serialization
{
	public static class ControllerExtensions
	{
		public static ActionResult CamelCaseJsonForAngular(this Controller controller, object data)
		{
			return new CamelCaseJsonResult(data);
		}

		public static ActionResult CamelCaseJsonForAngular(object data)
		{
			return new CamelCaseJsonResult(data);
		}

		internal static ActionResult CamelCaseJsonForAngular(Hub result, JsonRequestBehavior allowGet)
		{
			throw new NotImplementedException();
		}
	}
}