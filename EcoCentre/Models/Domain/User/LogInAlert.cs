using System;
using EcoCentre.Models.Infrastructure;
using NLog;
using Logger = NLog.Logger;

namespace EcoCentre.Models.Domain.User
{
	public class LogInAlert
	{
		private readonly Mailer _mailer;
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public LogInAlert(Mailer mailer)
		{
			_mailer = mailer;
		}

		public string HubName { get; set; }
		public string UserName { get; set; }
		public string DestinationAddress { get; set; }
		public DateTime LogInDateUtc { get; set; }

		public bool TrySend()
		{
			try
			{
				var logInDate = LogInDateUtc.ToLocalDateTime();
				var message = $"L’employé {UserName} s’est connecter le {logInDate:d} à {logInDate:t}.";
				var subject = $"Écocentre {HubName} connexion";

				_mailer.SendPlain(DestinationAddress, subject, message);

				return true;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}

			return false;
		}
	}
}