﻿using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace EcoCentre
{
	public class LocalizationAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.RouteData.Values["lang"] != null &&
			    !string.IsNullOrWhiteSpace(filterContext.RouteData.Values["lang"].ToString()))
			{
				// set the culture from the route data (url)
				var lang = filterContext.RouteData.Values["lang"].ToString();
				Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
			}
			else
			{
				// load the culture info from the cookie
				var cookie = filterContext.HttpContext.Request.Cookies["EcoCentre.UICulture"];
				string langHeader = null;
				if (cookie != null)
				{
					// set the culture by the cookie content
					langHeader = cookie.Value;
					Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
				}
				else
				{
					// set the culture by the location if not speicified
					/*if (filterContext.HttpContext.Request.UserLanguages != null)
                        langHeader = filterContext.HttpContext.Request.UserLanguages[0];
                    if (langHeader != null) Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
                    */

					// set default language
					langHeader = "fr-CA";
					Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
				}

				// set the lang value into route data
				filterContext.RouteData.Values["lang"] = langHeader;
			}

			// save the location into cookie
			var _cookie = new HttpCookie("EcoCentre.UICulture",
				Thread.CurrentThread.CurrentUICulture.Name)
			{
				Expires = DateTime.Now.AddYears(1)
			};
			filterContext.HttpContext.Response.SetCookie(_cookie);

			base.OnActionExecuting(filterContext);
		}
	}
}