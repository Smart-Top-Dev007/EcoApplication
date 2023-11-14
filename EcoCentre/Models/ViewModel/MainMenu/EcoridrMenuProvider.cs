using EcoCentre.Models.Domain.User;
using EcoCentre.Models.Infrastructure.SystemSettings;

namespace EcoCentre.Models.ViewModel.MainMenu
{
	public class EcoridrMenuProvider : MenuProvider
	{
		public EcoridrMenuProvider(AuthenticationContext authenticationContext, SystemSettings settings) : base(authenticationContext, settings)
		{
			Menu.Add(new MenuItem(authenticationContext.User, "ecoridr.ca", "http://ecoridr.ca", true, true));
		}
	}
}