using System.Web.Mvc;
using EcoCentre.Models.Infrastructure;

namespace EcoCentre
{
	public class DomainErrorsHandler : FilterAttribute, IExceptionFilter
	{
		public void OnException(ExceptionContext filterContext)
		{
			if (!(filterContext.Exception is DomainException exception))
			{
				return;
			}


			filterContext.ExceptionHandled = true;

			if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
			{
				var result = new JsonResult
				{
					Data = new[] {new {ErrorMessage = exception.Message}},
					JsonRequestBehavior = JsonRequestBehavior.AllowGet
				};
				filterContext.Result = result;
			}
			else
			{
				var result = new ContentResult()
				{
					Content = exception.Message
				};
				filterContext.Result = result;
			}

			var responseStatusCode = 500;
			if (filterContext.Exception is NotFoundException)
			{
				responseStatusCode = 404;
			}

			filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
			filterContext.HttpContext.Response.StatusCode = responseStatusCode;
		}
	}
}