using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EcoCentre.Models.Infrastructure;
using System.Web;
using System.Web.Security;
using EcoCentre.Models.Domain.User;

namespace EcoCentre.Controllers
{
    public class DefaultController : Controller
    {
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        //
        // GET: /Default/

        public ActionResult Index(AuthenticationContext authenticationContext)
        {
            //if(_CheckAuthenticationContextForUserName)
            if (null == authenticationContext.User)
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }
        public ActionResult DashboardTemplate()
        {
            return View();
        }
        public ActionResult Localizations()
        {
	        var data = new
	        {
		        frCA = new Dictionary<string, string>
		        {
			        {"true", "Oui"},
			        {"false", "Non"},
			        {"Do you want to remove the user?", "Voulez-vous supprimer l'utilisateur?"},
			        {"Do you want to remove the invoice?", "Voulez-vous supprimer la facture?"},
			        {"Resident", Resources.Model.Client.CategoryResident},
			        {"Other", Resources.Model.Client.CategoryOther},
			        {"Institution", Resources.Model.Client.CategoryInstitution},
			        {"Municipality", Resources.Model.Client.CategoryMunicipality},
			        {"OBNL", Resources.Model.Client.CategoryOBNL},
			        {
				        "This address already exists in the database. Are you sure you want to create a customer at this address?",
				        "L'adresse  est déjà enregistrée. Êtes-vous sûr de vouloir créer un nouveau client à cette adresse?"
			        }
		        }
	        };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogError(string error, string url, string lineNumber)
        {
            Elmah.ErrorSignal.FromCurrentContext().Raise(new ClientSideException(error, url, lineNumber));
            return Content("");
        }
    }


}
