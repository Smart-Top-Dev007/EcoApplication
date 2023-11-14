using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using EcoCentre.Models.Commands.Scheduler;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.User;
using EcoCentre.Models.ViewModel.MainMenu;
using EcoCentre.Web;
using FluentValidation;
using MassTransit;
using Module = Autofac.Module;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Invoices.Commands;
using EcoCentre.Models.Infrastructure.SystemSettings;
using EcoCentre.Models.MoneticoPayments;
using EcoCentre.Models.Queries;
using Newtonsoft.Json;
using NLog;

namespace EcoCentre.Models.Infrastructure
{
	public class DomainModule : Module
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		public string ConnectionString { get; set; }
		public string LocalDatabaseName { get; set; }
		public string GlobalDatabaseName { get; set; }
		public string OtherDatabases { get; set; }
		public string CenterName { get; set; }
		public string CenterUrl { get; set; }
		public string InvoicePattern { get; set; }
		public string MinBackgroundTaskInterval { get; set; }
		public int MaxMaterialByAddressCachedRequestsNumber { get; set; }
		public int MaxMaterialByAddressCachedRequestAgeInDays { get; set; }
		public int LastSeenOnlineUpdateIntervalInMinutes { get; set; }

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<MenuProvider>().As<IMenuProvider>();
			builder.RegisterType<InvoiceMapper>();
			builder.RegisterGeneric(typeof(Repository<>));

			builder.RegisterAssemblyTypes(Assembly.GetAssembly(GetType()))
				   .Where(x => x.IsDerivativeOfGeneric(typeof(AbstractValidator<>)))
				   .InstancePerHttpRequest();

			var autoregisterClassEndsWith = new[] { "Query", "Command", "Formatter" };
			builder.RegisterAssemblyTypes(Assembly.GetAssembly(GetType()))
				.Where(
					x =>
						x.Namespace != null &&
						x.Namespace.Contains("EcoCentre") &&
						autoregisterClassEndsWith.Any(end => x.Name.EndsWith(end)))
				.AsSelf();

			builder.RegisterAssemblyTypes(Assembly.GetAssembly(GetType()))
				   .Where(x => x.GetInterfaces().Any(t => t == typeof(ITask)))
				   .InstancePerHttpRequest().As<ITask>().AsSelf();

			builder.RegisterAssemblyTypes(Assembly.GetAssembly(GetType()))
				   .Where(x => x.GetInterfaces().Any(t => t == typeof(IAdminTask)))
				   .SingleInstance().As<IAdminTask>().AsSelf();

			builder.RegisterType<AuthenticationContext>().InstancePerHttpRequest();
			builder.RegisterType<Sequences>();
			builder.RegisterType<FileRepository>();
			builder.RegisterType<TaskRepository>();
			builder.RegisterType<EcoWebPageView>();
			builder.RegisterType<Mailer>();
			builder.RegisterType<ClosedSessionTracker>().SingleInstance();
			builder.RegisterType<EcoAuthorizeAttribute>();
			builder.RegisterType<CloseSessionHandler>();
			builder.RegisterType<SessionTrackerHandler>()
				.WithProperty(nameof(SessionTrackerHandler.UpdateIntervalInMinutes), LastSeenOnlineUpdateIntervalInMinutes);

			builder.RegisterGeneric(typeof(EcoWebPageView<>));

			builder.Register(ctx =>
				{
					var context = ctx.Resolve<AuthenticationContext>();
					return new CenterIdentification
					{
						Name = context.Hub?.Name ?? CenterName,
						Id = context.Hub?.Id,
						Url = CenterUrl,
						Address = context.Hub?.Address
					};
				})
				.InstancePerHttpRequest();

			builder.Register(ctx =>
			{
				var context = ctx.Resolve<AuthenticationContext>();
				return new InvoiceFormatter(InvoicePattern, context.Hub);
			}).InstancePerHttpRequest();

			builder.Register(ctx => new BgTaskInterval
			{
				Interval = TimeSpan.Parse(MinBackgroundTaskInterval)
			}).InstancePerLifetimeScope();

			builder.Register(ctx => new MaterialByAddressCachingSettings
			{
				MaxCachedRequestsNumber = MaxMaterialByAddressCachedRequestsNumber,
				MaxCachedRequestAgeInDays = MaxMaterialByAddressCachedRequestAgeInDays
			}).InstancePerLifetimeScope();

			builder.RegisterAssemblyTypes(typeof(DomainModule).Assembly)
				.Where(type => type.GetInterfaces().Contains(typeof(IConsumer)))
				.AsSelf();

			builder.RegisterType<PaymentProcessor>();
			builder.RegisterType<PaymentResponseValidator>();

			var databaseConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EcoDatabase"].ConnectionString;

			builder.Register(ctx => new UnitOfWork(databaseConnectionString)).InstancePerLifetimeScope();

			var serviceBusConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ServiceBus"].ConnectionString;

			builder.Register(c => ServiceBusFactory.New(
				sbc =>
				{
					//sbc.SetDefaultRetryLimit(0);

					if (serviceBusConnectionString.StartsWith("msmq://"))
					{
						sbc.UseMsmq();
					}
					else if (!serviceBusConnectionString.StartsWith("loopback://"))
					{
						throw new Exception("MassTransit supports only MSMQ and LOOPBACK transports.");
					}

					sbc.ReceiveFrom(serviceBusConnectionString);
					sbc.Subscribe(x => x.LoadFrom(c.Resolve<ILifetimeScope>()));
					sbc.Subscribe(x => x.Handler<IFault>(f =>
					{
						ErrorLog.Log(new Exception(f.Messages.FirstOrDefault()));
						Logger.Error($"Error when handing an event. Details: {JsonConvert.SerializeObject(f)}");
					}));
				}))
				.As<IServiceBus>()
				.SingleInstance();
			builder.RegisterFilterProvider();

			builder
				.Register(c =>
				{
					var query = c.Resolve<SystemSettingsQuery>();
					return query.Execute();
				})
				.As<SystemSettings.SystemSettings>()
				.SingleInstance();
		}
	}

	public class ClosedSessionTracker
	{
		private readonly HashSet<string> _logins = new HashSet<string>();
		public void CloseSession(string login)
		{
			_logins.Add(login);
		}

		public bool Contains(string login)
		{
			return _logins.Contains(login);
		}

		public void Remove(string login)
		{
			_logins.Remove(login);
		}
	}
}