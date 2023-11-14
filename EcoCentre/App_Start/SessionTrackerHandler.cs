using System;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Mvc;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.User;
using NLog;

namespace EcoCentre
{
	public class SessionTrackerHandler : ActionFilterAttribute
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly Repository<User> _userRepository;

		private readonly MemoryCache _cache = new MemoryCache("userSessions");

		public int UpdateIntervalInMinutes { get; set; }

		public SessionTrackerHandler(Repository<User> userRepository)
		{
			_userRepository = userRepository;
		}
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var login = filterContext.RequestContext.HttpContext.User?.Identity?.Name?.ToLower();
			if (string.IsNullOrWhiteSpace(login))
			{
				return;
			}

			var noUpdatesRecently = _cache[login] == null;
			if (noUpdatesRecently)
			{
				try
				{
					UpdateOnlineDate(login, filterContext);
					_cache.Add(login, login, DateTimeOffset.Now.AddMinutes(UpdateIntervalInMinutes));
				}
				catch (Exception ex)
				{
					Logger.Error(ex, $"Failed to update last seen online date for user {login}");
				}
			}

		}

		private void UpdateOnlineDate(string login, ActionExecutingContext filterContext)
		{
			var user = _userRepository.Query.Single(x => x.LoginLower == login);
			user.LastSeenOnline = DateTime.UtcNow;

			user.LastSeenOnlineOnHub = filterContext.HttpContext.Request.Cookies[AuthenticationContext.HubidCookie]?.Value;

			_userRepository.Save(user);
		}
	}
}