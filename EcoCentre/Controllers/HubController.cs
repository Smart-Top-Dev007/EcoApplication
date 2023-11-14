using System.Web.Mvc;
using EcoCentre.Models.Domain.Hubs.Commands;
using EcoCentre.Models.Domain.Hubs.Queries;
using EcoCentre.Models.Domain.User;
using static EcoCentre.Models.Infrastructure.Serialization.ControllerExtensions;

namespace EcoCentre.Controllers
{
    using System;

    public class HubController : Controller
    {
	    private readonly AuthenticationContext _authenticationContext;

	    public HubController(AuthenticationContext authenticationContext)
	    {
		    _authenticationContext = authenticationContext;
	    }

	    protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        public ActionResult NewTemplate()
        {
            return View();
        }
        public ActionResult IndexTemplate()
        {
            return View();
        }

        public ActionResult Index(HubDetailsQuery detailsQuery, HubListQuery listQuery)
        {
            if(!string.IsNullOrEmpty(detailsQuery.Id))
            {
                var result = detailsQuery.Execute();
                return CamelCaseJsonForAngular(result);
            }
            var data = listQuery.Execute();
            return CamelCaseJsonForAngular(data);
        }


	    public ActionResult Current()
	    {
		    var data = _authenticationContext.Hub;
		    return CamelCaseJsonForAngular(data);
	    }


		[RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Put)]
        public ActionResult Index(UpdateHubCommand command)
        {
            command.Execute();
            return Json("Ok");
        }

        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(CreateHubCommand command)
        {
            command.Execute();
            return Json("Ok");
        }

    }
}
