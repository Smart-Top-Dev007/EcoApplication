using System.Web.Mvc;
using FluentValidation;
using Newtonsoft.Json;
using NLog;

namespace EcoCentre
{
	public class HandleValidationErrorsHandler : FilterAttribute, IExceptionFilter
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public void OnException(ExceptionContext filterContext)
		{
			if (!(filterContext.Exception is ValidationException exception))
			{
				return;
			}

			var request = filterContext.RequestContext.HttpContext.Request;
			if (!request.IsAjaxRequest())
			{
				return;
			}

			filterContext.ExceptionHandled = true;

			var result = new JsonResult
			{
				Data = exception.Errors,
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
			filterContext.Result = result;


			Logger.Info(
				$"Invalid request. URL: {request.Url}, Method: {request.HttpMethod}, Exception: {JsonConvert.SerializeObject(exception, Formatting.Indented)}");


			filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
			filterContext.HttpContext.Response.StatusCode = 409;
		}
	}
}