using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EcoCentre
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Template",
				url: "template/{folder}/{file}",
				defaults: new { controller = "Template", action = "Index" });


			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional },
				constraints: new { controller = new ControllerNameConstraint() });

			routes.MapRoute(
				name: "404-PageNotFound",
				url: "{*url}",
				defaults: new { controller = "Error", action = "NotFound" }
			);
		}

		public class ControllerNameConstraint : IRouteConstraint
		{
			private readonly List<string> _controllerNames;

			public ControllerNameConstraint()
			{
				_controllerNames = GetControllerNames();
			}
			private static IEnumerable<Type> GetSubClasses<T>()
			{
				return Assembly.GetCallingAssembly().GetTypes()
					.Where(type => type.IsSubclassOf(typeof(T)));
			}

			public List<string> GetControllerNames()
			{
				return GetSubClasses<Controller>()
					.Select(type => type.Name.Replace("Controller", "").ToLower())
					.ToList();
			}
			public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
			{
				if (values.ContainsKey(parameterName))
				{
					var stringValue = values[parameterName] as string;
					return _controllerNames.Contains(stringValue?.ToLower());
				}

				return false;
			}
		}
	}
}