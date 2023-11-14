using System;
using EcoCentre.Models.Domain.User.Consumers;
using EcoCentre.Models.Infrastructure;
using NLog;

namespace EcoCentre.Models.Domain.User
{
	public class LogoutAlert
	{
		private readonly Mailer _mailer;
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public LogoutAlert(Mailer mailer)
		{
			_mailer = mailer;
		}

		public string HubName { get; set; }
		public string UserName { get; set; }
		public DateTime LogoutDateUtc { get; set; }
		public string DestinationAddress { get; set; }
		public LogoutType LogoutType { get; set; }

		public bool TrySend()
		{
			try
			{
				var localLogoutDate = LogoutDateUtc.ToLocalDateTime();
				var logoutType = LogoutType == LogoutType.SessionTimeout ? " (Expiration de la session)" : "";
				var message = $"L’employé {UserName} s’est deconnecter le {localLogoutDate:d} à {localLogoutDate:t}{logoutType}.";
				var subject = $"Écocentre {HubName} deconnexion";

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