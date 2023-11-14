using System.Web.Mvc;
using MembershipService.Common;

namespace EcoCentre
{
	public class EcoAuthorizeAttribute : AuthorizeAttribute
	{
		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			base.HandleUnauthorizedRequest(filterContext);
			if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
				DoNotRedirectToLoginModule.ApplyForRequest(filterContext.HttpContext);
		}
	}
}