using System.Collections.Generic;

namespace EcoCentre.Models.ViewModel.MainMenu
{
	public interface IMenuProvider
	{
		IList<MenuItem> Menu { get; }
	}
}