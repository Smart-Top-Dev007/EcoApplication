using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.Infrastructure;
using MassTransit;

namespace EcoCentre.Models.Domain.User.Consumers
{
	public class SendAlertOnUserLoggedIn : Consumes<UserLoggedInEvent>.All
	{
		private readonly Repository<Hub> _hubRepository;
		private readonly Repository<User> _userRepository;
		private readonly Mailer _mailer;

		public SendAlertOnUserLoggedIn(Repository<Hub> hubRepository, Repository<User> userRepository, Mailer mailer)
		{
			_hubRepository = hubRepository;
			_userRepository = userRepository;
			_mailer = mailer;
		}
		public void Consume(UserLoggedInEvent message)
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

			var logInAlert = new LogInAlert(_mailer)
			{
				UserName = !string.IsNullOrWhiteSpace(user.Name) ? user.Name : user.Login,
				LogInDateUtc = DateTime.UtcNow,
				HubName = hub.Name,
				DestinationAddress = hub.EmailForLoginAlerts
			};
			logInAlert.TrySend();
		}
	}
}
