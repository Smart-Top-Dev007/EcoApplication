using System.Web.Mvc;
using Autofac;

namespace EcoCentre
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters, IContainer container)
		{
			filters.Add(new ErrorHandler.AiHandleErrorAttribute());
			filters.Add(new LocalizationAttribute());
			filters.Add(container.Resolve<EcoAuthorizeAttribute>());
			filters.Add(new HandleValidationErrorsHandler());
			filters.Add(new DomainErrorsHandler());
			filters.Add(container.Resolve<SessionTrackerHandler>());
			filters.Add(container.Resolve<CloseSessionHandler>());
		}
	}
}