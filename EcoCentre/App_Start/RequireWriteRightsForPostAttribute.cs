using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using EcoCentre.Models.Domain.User;
using FluentValidation;
using FluentValidation.Results;

namespace EcoCentre
{
	public class RequireWriteRightsForPostAttribute : AuthorizeAttribute
	{
		public AuthenticationContext CurAutenticationContext { get; set; }

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			//return CurAutenticationContext.User.IsGlobalAdmin;
			var httpMethodLower = httpContext.Request.HttpMethod.ToLower();

			if (httpMethodLower != "get")
			{
				return !CurAutenticationContext.User.IsReadOnly;
			}

			return true;
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			base.HandleUnauthorizedRequest(filterContext);
			throw new ValidationException(new List<ValidationFailure>
			{
				new ValidationFailure("save", Resources.Model.User.UserIsReadOnly)
			});
//            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
//            {
//                DoNotRedirectToLoginModule.ApplyForRequest(filterContext.HttpContext);
//            }
//            else
//            {
//                filterContext.Result = new RedirectToRouteResult(
//                                      new RouteValueDictionary 
//                                   {
//                                       { "action", "Index" },
//                                       { "controller", "Default" }
//                                   });
//            }
			//if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
			//    DoNotRedirectToLoginModule.ApplyForRequest(filterContext.HttpContext);
		}
	}
}