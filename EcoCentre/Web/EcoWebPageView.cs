using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.Domain.User;
using EcoCentre.Models.Infrastructure.SystemSettings;
using EcoCentre.Models.ViewModel;
using EcoCentre.Models.ViewModel.MainMenu;

namespace EcoCentre.Web
{
	public abstract class EcoWebPageView<T> : WebViewPage<T>
	{
		public AuthenticationContext AuthenticationContext { get; set; }
		public User CurrentUser => AuthenticationContext.User;
		public Hub CurrentHub => AuthenticationContext.Hub;
		public IMenuProvider MenuProvider { get; set; }
		public ICustomizationProvider Customization { get; set; }
		public SystemSettings SystemSettings { get; set; }

		public string BaseUrl
		{
			get
			{
				var url = Request.Url;
				var path = $"{Request.Url.Scheme}://{url.Host}";
				if (!url.IsDefaultPort)
					path = path + ":" + url.Port;
				return path;
			}
		}
		

	}
	public abstract class EcoWebPageView : EcoWebPageView<object>
	{
	}
}