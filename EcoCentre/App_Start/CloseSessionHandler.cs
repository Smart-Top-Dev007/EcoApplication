using System.Runtime.Caching;
using System.Web.Mvc;
using System.Web.Security;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.User;
using EcoCentre.Models.Infrastructure;
using NLog;

namespace EcoCentre
{
	public class CloseSessionHandler : ActionFilterAttribute
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


		private readonly ClosedSessionTracker _closedSessionTracker;


		public CloseSessionHandler(ClosedSessionTracker closedSessionTracker)
		{
			_closedSessionTracker = closedSessionTracker;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{

			var login = filterContext.RequestContext.HttpContext.User?.Identity?.Name?.ToLower();
			if (string.IsNullOrWhiteSpace(login))
			{
				return;
			}

			if (_closedSessionTracker.Contains(login))
			{
				_closedSessionTracker.Remove(login);
				FormsAuthentication.SignOut();
			}
		}
	}
}