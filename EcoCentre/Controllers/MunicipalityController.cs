using System.Web.Mvc;
using EcoCentre.Models.Commands;
using EcoCentre.Models.Domain.Municipalities.Commands;
using EcoCentre.Models.Domain.Municipalities.Queries;
using EcoCentre.Models.Infrastructure.Serialization;

namespace EcoCentre.Controllers
{
    using System;

    public class MunicipalityController : Controller
    {
        private readonly CreateMunicipalityCommand _createMunicipalityCommand;
        private readonly MunicipalityListQuery _municipalityListQuery;
        private readonly UpdateMunicipalityCommand _updateMunicipalityCommand;
        private readonly MunicipalityDetailsQuery _municipalityDetailsQuery;

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

        public MunicipalityController(CreateMunicipalityCommand createMunicipalityCommand, MunicipalityListQuery municipalityListQuery, 
            UpdateMunicipalityCommand updateMunicipalityCommand, MunicipalityDetailsQuery municipalityDetailsQuery)
        {
            _createMunicipalityCommand = createMunicipalityCommand;
            _municipalityListQuery = municipalityListQuery;
            _updateMunicipalityCommand = updateMunicipalityCommand;
            _municipalityDetailsQuery = municipalityDetailsQuery;
        }

        public ActionResult NewTemplate()
        {
            return View();
        }
        public ActionResult IndexTemplate()
        {
            return View();
        }

        public ActionResult Index(MunicipalityListParams param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                var result = _municipalityListQuery.Execute();
                return new CamelCaseJsonResult(result);
            }
            var data = _municipalityDetailsQuery.Execute(param.Id);
            return new CamelCaseJsonResult(data);
        }
        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Put)]
        public ActionResult Index(string id, string name, bool enabled, string hubId)
        {
            _updateMunicipalityCommand.Execute(id, name, enabled, hubId);
            return Json("Ok");
        }

        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(string name)
        {
            _createMunicipalityCommand.Execute(name);
            return Json("Ok");
        }

    }
}
