using System;
using System.Web.Security;
using EcoCentre.Models.Domain.User.Consumers;
using MassTransit;
using NLog;

namespace EcoCentre.Models.Domain.User.Commands
{
	public class LogoutCommand
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly AuthenticationContext _authenticationContext;
		
		private readonly Repository<User> _userRepository;
		private readonly IServiceBus _serviceBus;

		public LogoutCommand(AuthenticationContext authenticationContext, Repository<User> userRepository, IServiceBus serviceBus)
		{
			_authenticationContext = authenticationContext;
			_userRepository = userRepository;
			_serviceBus = serviceBus;
		}

		public void Execute()
		{
			FormsAuthentication.SignOut();

			var hub = _authenticationContext.Hub;
			var user = _authenticationContext.User;

			_serviceBus.Publish(new UserLoggedOutEvent
			{
				UserId = user.Id,
				//HubId = hub.Id,
				LogoutType = LogoutType.UserInitiated
			});

			var userToUpdate = _userRepository.FindOne(user.Id);
			userToUpdate.LogoutDate = DateTime.UtcNow;
			_userRepository.Save(userToUpdate);

		}

	}
}