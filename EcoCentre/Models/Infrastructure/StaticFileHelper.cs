using System.IO;
using System.Web;
using System.Web.Mvc;

namespace EcoCentre.Models.Infrastructure
{
	public static class StaticFileHelper
	{
		public static string StaticFile(this UrlHelper html, string filename)
		{
			var virtualPath = GetPath(filename, html.RequestContext.HttpContext);
			
			var root = html.RequestContext.HttpContext.Request.ApplicationPath;
			if (root.Length > 1)
			{
				virtualPath = root + virtualPath;
			}
			return virtualPath;
		}
		
		static string GetPath(string filename, HttpContextBase context)
		{
			var absoluteFilename = context.Server.MapPath("~/" + filename);
			var version = GetFileWriteTime(absoluteFilename).ToString();
			var separator = filename.Contains("?") ? "&" : "?";
			return filename + separator + "nocache=" + version;
		}

		private static long GetFileWriteTime(string absoluteFilename)
		{
			return File.GetLastWriteTime(absoluteFilename).Ticks;
		}
	}
}