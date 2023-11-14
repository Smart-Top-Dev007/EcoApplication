using System.Linq;
using System.Web;
using EcoCentre.Models.Domain.Hubs;

namespace EcoCentre.Models.Domain.User
{
    public class AuthenticationContext
    {
	    public const string HubidCookie = "hubid";
	    private readonly HttpContextBase _context;
        private readonly Repository<User> _userRepository;
	    private readonly Repository<Hub> _hubRepository;

	    public AuthenticationContext(HttpContextBase context, Repository<User> userRepository, Repository<Hub> hubRepository)
        {
            _context = context;
            _userRepository = userRepository;
	        _hubRepository = hubRepository;
        }

        private User _user;
        public User User
        {
            get { return _user ?? (_user = _userRepository.Query.SingleOrDefault(x => x.LoginLower == _context.User.Identity.Name.ToLower())); }
        }

		private Hub _hub;
        public Hub Hub
        {
	        get => _hub ?? (_hub = GetHub());
	        set {
				_context.Response.SetCookie(new HttpCookie(HubidCookie, value?.Id));
		        _hub = value;
	        }
        }

	    public string UserAgent => _context?.Request.UserAgent;

	    private Hub GetHub()
	    {
		    var value = _context.Request.Cookies[HubidCookie]?.Value;
		    if (string.IsNullOrWhiteSpace(value))
		    {
			    return null;
		    }
		    return _hubRepository.Query.SingleOrDefault(x => x.Id == value);
	    }
    }
}