using System.Web.Mvc;
using EcoCentre.Models.Domain.Limits.Queries;
using EcoCentre.Models.Queries;
using EcoCentre.Models.Domain.GlobalSettings.Queries;
using EcoCentre.Models.Domain.Clients;
using MongoDB.Driver;
using System;
using System.Linq;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Clients.Commands;

namespace EcoCentre.Controllers
{
    using System;

    public class LimitsController : Controller
    {
        private readonly LimitsDetailsQuery _limitsDetailsQuery;
        private readonly LimitsListQuery _limitsListQuery;
		private readonly GlobalSettingsQuery _globalSettingsQuery;
		private readonly Repository<Client> _clientRepository;
		private readonly UpdateClientCommand _updateClientCommand;

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

        public LimitsController(Repository<Client> clientRepository,
			 UpdateClientCommand updateClientCommand,
			  LimitsDetailsQuery limitsDetailsQuery, LimitsListQuery limitsListQuery, GlobalSettingsQuery globalSettingsQuery)
        {
            _limitsDetailsQuery = limitsDetailsQuery;
            _limitsListQuery = limitsListQuery;
			_globalSettingsQuery = globalSettingsQuery;
			_clientRepository = clientRepository;
			_updateClientCommand = updateClientCommand;
		}

        public ActionResult Index(LimitsListQueryParams param)
        {
			var globalSettings = _globalSettingsQuery.Execute();
			if (param.Id == null)
            {
                var limits = _limitsListQuery.Execute(param);

				if(globalSettings.MaxYearlyClientVisits == limits.CurrentLimits.Count)
				{
					var rawClient = _clientRepository.Collection.AsQueryable().SingleOrDefault(x => x.Id == param.ClientId);
					rawClient.VisitsLimitExceeded = true;
					_clientRepository.Save(rawClient);
				}
                return Json(limits, JsonRequestBehavior.AllowGet);
            }
            var result = _limitsDetailsQuery.Execute(param.Id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}