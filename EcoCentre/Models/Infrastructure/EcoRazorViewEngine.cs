using System.Linq;
using System.Web.Mvc;

namespace EcoCentre.Models.Infrastructure
{
	public class EcoRazorViewEngine : RazorViewEngine
	{
		private const string DefaultViewPathFormat = "/{1}/{0}.cshtml";
		private const string DefaultSharedViewPathFormat = "/Shared/{0}.cshtml";
		public EcoRazorViewEngine(string customRoot)
		{
			if (string.IsNullOrEmpty(customRoot)) return;
			var customPath = customRoot + DefaultViewPathFormat;
			var customSharedPath = customRoot + DefaultSharedViewPathFormat;

			var list = ViewLocationFormats.ToList();
			list.Insert(0, customSharedPath);
			list.Insert(0, customPath);
			ViewLocationFormats = list.ToArray();

			list = PartialViewLocationFormats.ToList();
			list.Insert(0, customSharedPath);
			list.Insert(0, customPath);
			PartialViewLocationFormats = list.ToArray();

            
		}
	}
}