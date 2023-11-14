using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EcoCentre.Models.Domain.User;
using MembershipService.Common;

namespace EcoCentre
{
	public class RequireGlobalAdminAttribute : AuthorizeAttribute
	{
		public AuthenticationContext CurAutenticationContext { get; set; }

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			//return CurAutenticationContext.User.IsGlobalAdmin;
			return CurAutenticationContext.User.IsGlobalAdmin;
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			base.HandleUnauthorizedRequest(filterContext);
			if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
			{
				DoNotRedirectToLoginModule.ApplyForRequest(filterContext.HttpContext);
			}
			else
			{
				filterContext.Result = new RedirectToRouteResult(
					new RouteValueDictionary
					{
						{"action", "Index"},
						{"controller", "Default"}
					});
			}

			//if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
			//    DoNotRedirectToLoginModule.ApplyForRequest(filterContext.HttpContext);
		}
	}
}