using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using EcoCentre.Models.Domain.Clients.Commands;
using EcoCentre.Models.Domain.GlobalSettings.Queries;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.Domain.User.Consumers;
using EcoCentre.Models.Infrastructure;
using FluentValidation;
using FluentValidation.Results;
using MassTransit;
using NLog;

namespace EcoCentre.Models.Domain.User.Commands
{
	public class LoginCommand
	{
		private readonly Repository<User> _userRepository;
		private readonly LoginDataValidator _validator;
		private readonly Repository<Hub> _hubRepository;
		private readonly AuthenticationContext _authenticationContext;
		private readonly ClosedSessionTracker _closedSessionTracker;
		private readonly Repository<GlobalSettings.GlobalSettings> _settingsRepository;
		private readonly IServiceBus _serviceBus;

		public LoginCommand(
			Repository<User> userRepository,
			LoginDataValidator validator,
			Repository<Hub> hubRepository,
			AuthenticationContext authenticationContext,
			ClosedSessionTracker closedSessionTracker,
			Repository<GlobalSettings.GlobalSettings> settingsRepository,
			IServiceBus serviceBus)
		{
			_userRepository = userRepository;
			_validator = validator;
			_hubRepository = hubRepository;
			_authenticationContext = authenticationContext;
			_closedSessionTracker = closedSessionTracker;
			_settingsRepository = settingsRepository;
			_serviceBus = serviceBus;
		}

		public void Execute(LoginData loginData)
		{
			_validator.ValidateAndThrow(loginData);
			var login = loginData.Login.ToLower();
			var user = _userRepository.Query.SingleOrDefault(x => x.LoginLower == login);
			int newTimeout = 60;
			if (user == null || !user.VerifyPassword(loginData.Password))
			{
				var failure = new ValidationFailure("login", Resources.Model.User.InvalidUsernameOrPassword);
				throw new ValidationException(failure.AsList());
			}
			var globalSettings = _settingsRepository.Query.First();

			if (globalSettings.SessionTimeoutInMinutes.HasValue)
			{
				newTimeout = Convert.ToInt32(globalSettings.SessionTimeoutInMinutes);
			}

			user.LastSeenOnlineOnHub = loginData.HubId;
			user.LastSeenOnline = DateTime.UtcNow;
			user.LastLoginAt = DateTime.UtcNow;

			_userRepository.Save(user);

			_closedSessionTracker.Remove(login);

			FormsAuthentication.SetAuthCookie(user.LoginLower, false);

			// Get the current authentication ticket
			var currentTicket = FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value);

			// Create a new FormsAuthenticationTicket with the updated expiration time
			var newTicket = new FormsAuthenticationTicket(
				currentTicket.Version,
				currentTicket.Name,
				currentTicket.IssueDate,
				DateTime.Now.AddMinutes(newTimeout),
				currentTicket.IsPersistent,
				currentTicket.UserData,
				currentTicket.CookiePath
			);

			// Encrypt the new ticket
			string encryptedNewTicket = FormsAuthentication.Encrypt(newTicket);

			// Create a new authentication cookie with the updated ticket
			var newAuthCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedNewTicket);
			newAuthCookie.Expires = newTicket.Expiration;

			// Add the new cookie to the response
			HttpContext.Current.Response.Cookies.Add(newAuthCookie);

			var hub = _hubRepository.FindOne(loginData.HubId);

			_authenticationContext.Hub = hub;

			_serviceBus.Publish(new UserLoggedInEvent
			{
				UserId = user.Id,
				HubId = loginData.HubId
			});
		}
	}
}