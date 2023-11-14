
using System.Linq;
using EcoCentre.Models.Domain.Municipalities;

namespace EcoCentre.Models.Domain.User.Queries
{
    public class CurrentUserDetailsQuery
	{
		private readonly AuthenticationContext _authenticationContext;
		private readonly Repository<Municipality> _municipalityRepository;

        public CurrentUserDetailsQuery(AuthenticationContext authenticationContext, Repository<Municipality> municipalityRepository)
        {
	        _authenticationContext = authenticationContext;
	        _municipalityRepository = municipalityRepository;
        }

        public CurrentUserViewModel Execute()
        {
	        var hub = _authenticationContext.Hub;
	        var hubName = hub.Name.ToLower();
	        var city = _municipalityRepository.Query.FirstOrDefault(x => x.NameLower == hubName);
	        return new CurrentUserViewModel
	        {
		        HubId = hub.Id,
		        DefaultCityId = city?.Id
	        };

        }
    }
}