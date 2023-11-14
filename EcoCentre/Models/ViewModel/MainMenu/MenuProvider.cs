using System.Collections.Generic;
using EcoCentre.Models.Domain.User;
using EcoCentre.Models.Infrastructure.SystemSettings;

namespace EcoCentre.Models.ViewModel.MainMenu
{
    public class MenuProvider : IMenuProvider
    {
	    private readonly List<MenuItem> _menu;

        public IList<MenuItem> Menu => _menu;
		
	    public MenuProvider(AuthenticationContext authenticationContext, SystemSettings settings)
        {
	        var user = authenticationContext.User;
			
	        _menu = new List<MenuItem>
	        {
		        new MenuItem(user, "factures", "#", true, true)
		        {
			        SubItems = new List<MenuItem>
			        {
				        new MenuItem(user, "nouvelle facture", "#invoice/new", false, true),
				        new MenuItem(user, "recherche facture", "#invoice/index", true, true),
				        new MenuItem(user, "factures supprimés", "#invoice/trash", true, true)
			        }
		        },
		        new MenuItem(user, "clients", "#", true, true)
		        {
			        SubItems = new List<MenuItem>
			        {

				        new MenuItem(user, "nouveau client", "#client/new", false, true),
						new MenuItem(user, "nouveau canadian client", "#client/newcanadian", false, true),
						new MenuItem(user, "recherche client", "#client/index", true, true),
				        new MenuItem(user, "clients supprimés", "#client/inactive", true, true),
				        new MenuItem(user, Resources.Forms.MergeClients, "#client/merge", false, false, false)
			        }
		        },
		        new MenuItem(user, "matériaux", "#", true)
		        {
			        SubItems = new List<MenuItem>
			        {

				        new MenuItem(user, "nouveaux matériaux", "#material/new"),
				        new MenuItem(user, "liste des matériaux", "#material/index", true),
				        new MenuItem(user, Resources.Forms.MergeMaterials, "#material/merge", false, false, false)
			        }
		        },
		        new MenuItem(user, "rapports", "#", true)
		        {
			        SubItems = new List<MenuItem>
			        {

				        new MenuItem(user, "rapport de matériaux", "#report/materials", true),
				        new MenuItem(user, "limite de matériaux", "#report/limits", true),
				        new MenuItem(user, "limite de visites", "#report/visitslimits", true),
				        new MenuItem(user, "journal des transactions", "#report/journal", true),
				        new MenuItem(user, "matériaux par adresse", "#report/materialsbyaddress", true, false, false),

					}
		        },
		        new MenuItem(user, "administrateur", "#", true, false, false)
		        {
			        SubItems = new List<MenuItem>
			        {

				        new MenuItem(user, "utilisateurs", "#user/index", true, false, false),
				        new MenuItem(user, "municipalités", "#municipality/index", true, false, false),
				        new MenuItem(user, "regroupements", "#hub/index", true, false, false),
				        new MenuItem(user, "paiement", "#payment/settings", true, false, false),
				        new MenuItem(user, "paramètres globaux", "#globalsettings/index", false, false, false),
				        new MenuItem(user, "Test d'imprimante", "globalsettings/printerTest", false, false, false)
					}
		        }
	        };

	        if (settings.IsObnlEnabled)
	        {
		        _menu.Add(
			        new MenuItem(user, "OBNL", "#", true, true, false)
			        {
				        SubItems = new List<MenuItem>
				        {
					        new MenuItem(user, "OBNL Totale", "#report/obnltotal", true, false, false),
					        new MenuItem(user, "Matériaux Détails", "#report/obnlglobal", true, false, false),
					        new MenuItem(user, "OBNL réemploi", "#obnlreinvestment/new", false, true, false)
				        }
			        });
	        }
	        _menu.Add(
		        new MenuItem(user, "ton surplus... mon bonheur", "#", true, true)
		        {
			        SubItems = new List<MenuItem>
			        {
				        new MenuItem(user, "objets", "#giveaway/index", true, true),
				        new MenuItem(user, "types", "#giveaway-type/index", true, true),
			        }
		        });
			
	        _menu.Add(new MenuItem(user, "conteneurs", "#container/index", true, true));	
	        _menu.Add(new MenuItem(user, "se déconnecter", "/User/Logout", true, true));	
        }
    }
}