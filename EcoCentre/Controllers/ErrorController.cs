using System.Web.Mvc;

namespace EcoCentre.Controllers
{
	[AllowAnonymous]
	public sealed class ErrorController : Controller
	{
		public ActionResult NotFound()
		{
			object model = Request.Url.PathAndQuery;
			Response.StatusCode = 404;
			if (Request.IsAjaxRequest()) { 
				return new HttpNotFoundResult("Not found");
			}
			return  View(model);
		}
	}
}