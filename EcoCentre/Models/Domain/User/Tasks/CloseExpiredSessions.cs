using System;
using System.Linq;
using EcoCentre.Models.Commands.Scheduler;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.Domain.User.Consumers;
using EcoCentre.Models.Infrastructure;
using Magnum.Extensions;
using MassTransit;
using NLog;

namespace EcoCentre.Models.Domain.User.Tasks
{
	public class CloseExpiredSessions : ITask
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly Repository<User> _useRepository;
		private readonly Mailer _mailer;
		private readonly Repository<Hub> _hubRepository;
		private readonly ClosedSessionTracker _closedSessionTracker;
		private readonly Repository<GlobalSettings.GlobalSettings> _settingsRepository;
		private readonly IServiceBus _serviceBus;

		public CloseExpiredSessions(
			Repository<User> useRepository,
			Mailer mailer,
			Repository<Hub> hubRepository,
			ClosedSessionTracker closedSessionTracker,
			Repository<GlobalSettings.GlobalSettings> settingsRepository, IServiceBus serviceBus){
			_useRepository = useRepository;
			_mailer = mailer;
			_hubRepository = hubRepository;
			_closedSessionTracker = closedSessionTracker;
			_settingsRepository = settingsRepository;
			_serviceBus = serviceBus;
		}
		public void Execute(DateTime execTime, bool force = false)
		{
			var globalSettings = _settingsRepository.Query.First();

			if (!globalSettings.SessionTimeoutInMinutes.HasValue)
			{
				return;
			}

			var maxDate = DateTime.UtcNow.AddMinutes(-globalSettings.SessionTimeoutInMinutes.Value);

			var users = _useRepository.Query
				.Where(x => x.LastSeenOnline < maxDate)
				.ToList()
				.Where(x => x.LogoutDate == null || x.LogoutDate < x.LastSeenOnline)
				.ToList();

			foreach (var user in users)
			{
				if (user.LastSeenOnlineOnHub == null)
				{
					continue;
				}

				_serviceBus.Publish(new UserLoggedOutEvent
				{
					UserId = user.Id,
					HubId = user.LastSeenOnlineOnHub,
					LogoutType = LogoutType.SessionTimeout
				});

				var userToUpdate = _useRepository.FindOne(user.Id);
				userToUpdate.LogoutDate = DateTime.UtcNow;
				_useRepository.Save(userToUpdate);

				_closedSessionTracker.CloseSession(user.Login.ToLowerInvariant());
			}
		}
	}
}