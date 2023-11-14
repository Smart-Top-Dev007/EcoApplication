using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.Infrastructure;
using MassTransit;

namespace EcoCentre.Models.Domain.User.Consumers
{
	public class SendAlertOnUserLoggedOut : Consumes<UserLoggedOutEvent>.All
	{
		private readonly Repository<Hub> _hubRepository;
		private readonly Repository<User> _userRepository;
		private readonly Mailer _mailer;

		public SendAlertOnUserLoggedOut(Repository<Hub> hubRepository, Repository<User> userRepository, Mailer mailer)
		{
			_hubRepository = hubRepository;
			_userRepository = userRepository;
			_mailer = mailer;
		}
		public void Consume(UserLoggedOutEvent message)
		{
			var hub = _hubRepository.FindOne(message.HubId);

			if (string.IsNullOrWhiteSpace(hub.EmailForLoginAlerts))
			{
				return;
			}

			var user = _userRepository.FindOne(message.UserId);
			if (!user.IsLoginAlertEnabled)
			{
				return;
			}

			var logInAlert = new LogoutAlert(_mailer)
			{
				UserName = !string.IsNullOrWhiteSpace(user.Name) ? user.Name : user.Login,
				LogoutDateUtc = message.LogoutDateUtc ?? DateTime.UtcNow,
				HubName = hub.Name,
				DestinationAddress = hub.EmailForLoginAlerts,
				LogoutType = message.LogoutType
			};
			logInAlert.TrySend();
		}
	}
}
