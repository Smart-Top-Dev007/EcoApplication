using System;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Configuration;
using Autofac.Integration.Mvc;
using EcoCentre.Controllers;
using EcoCentre.Models.Infrastructure;
using FluentValidation;
using Microsoft.ApplicationInsights.Extensibility;
using NLog;

namespace EcoCentre
{
    public class MvcApplication : System.Web.HttpApplication
    {

	    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		protected void Application_Error(object sender, EventArgs e)
		{             
            var exception = Server.GetLastError();
			Logger.Error(exception);
		}

        protected void Application_Start()
        {
	        var aiKey = ConfigurationManager.AppSettings["ApplicationInsightsInstrumentationKey"];
	        if (string.IsNullOrWhiteSpace(aiKey))
	        {
		        TelemetryConfiguration.Active.DisableTelemetry = true;
	        }
	        else
	        {
				TelemetryConfiguration.Active.InstrumentationKey = aiKey;
			}
			
			Logger.Info("App started");
			var builder = new ContainerBuilder();
	        BundleConfig.RegisterBundles(BundleTable.Bundles);
			
			builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();
            builder.RegisterModule(new ConfigurationSettingsReader("autofac"));
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterSource(new ViewRegistrationSource());
            var container = builder.Build();
            var resolver = new AutofacDependencyResolver(container);
            ModelBinderProviders.BinderProviders.Clear();
            ModelBinderProviders.BinderProviders.Add(new YourModelBinderProvider());

			DependencyResolver.SetResolver(resolver);

            var ve = container.Resolve<RazorViewEngine>();
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(ve);

            AreaRegistration.RegisterAllAreas();
            var routes = RouteTable.Routes;
            routes.MapRoute(
            "root", // Route name
                "", // URL with parameters
                new { controller = "Default", action = "Index"} // Parameter defaults
            );
	        
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters, container);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (Response.RedirectLocation != null && Response.RedirectLocation.Contains("ReturnUrl"))
            {
                Response.RedirectLocation = "/User/Login";
            }
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
        {

			var culture = CultureInfo.CreateSpecificCulture("fr-CA");
	        CultureInfo.DefaultThreadCurrentCulture = culture;
	        CultureInfo.DefaultThreadCurrentUICulture = culture;
	        CultureInfo.CurrentUICulture = culture;
	        CultureInfo.CurrentCulture = culture;

        }
	}
    public class YourModelBinderProvider : IModelBinderProvider
    {
	    private static readonly Type DecimalType = typeof(decimal);
	    private static readonly Type DecimalNullableType = typeof(decimal?);
		
	    public IModelBinder GetBinder(Type modelType)
        {
	        if (modelType == DecimalType) return new DecimalModelBinder();
	        if (modelType == DecimalNullableType) return new DecimalModelBinder();
	        return new AutofacModelBinder();
		}
    }
    public class AutofacModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

            var item = DependencyResolver.Current.GetService(bindingContext.ModelType);
            if (item == null)
                return base.BindModel(controllerContext, bindingContext);

            var context = new ModelBindingContext(bindingContext);
            Func<object> modelAccessor = () => item;
            context.ModelMetadata = new ModelMetadata(new DataAnnotationsModelMetadataProvider(),
                                                      bindingContext.ModelMetadata.ContainerType, modelAccessor,
                                                      item.GetType(), bindingContext.ModelName);
            var result = base.BindModel(controllerContext, context);
            var type = typeof(AbstractValidator<>);
            var validatorType = type.MakeGenericType(bindingContext.ModelType);

	        if (DependencyResolver.Current.GetService(validatorType) is IValidator validator)
            {
                var valid = validator.Validate(result);
                if (!valid.IsValid)
                {
                    foreach (var error in valid.Errors)
                        bindingContext.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                }
            }
            return result;
        }
    }
}