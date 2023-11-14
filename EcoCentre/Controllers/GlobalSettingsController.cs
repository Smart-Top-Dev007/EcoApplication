using System.Web.Mvc;
using EcoCentre.Models.Domain.Limits.Queries;
using EcoCentre.Models.Queries;
using EcoCentre.Models.Domain.GlobalSettings;
using EcoCentre.Models.Domain.GlobalSettings.Commands;
using EcoCentre.Models.Domain.GlobalSettings.Queries;
using System.Collections.Generic;

namespace EcoCentre.Controllers
{
    using System;

    public class GlobalSettingsController : Controller
    {
        private readonly GlobalSettingsQuery _globalSettingsQuery;
        private readonly UpdateGlobalSettingsCommand _updateGlobalSettingsCommand;

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

        public GlobalSettingsController(GlobalSettingsQuery globalSettingsQuery, UpdateGlobalSettingsCommand updateGlobalSettingsCommand)
        {
            _globalSettingsQuery = globalSettingsQuery;
            _updateGlobalSettingsCommand = updateGlobalSettingsCommand;
        }

//        [RequireGlobalAdmin]
        public ActionResult MainForm()
        {
            return View();
        }

//        [RequireGlobalAdmin]
        public ActionResult Index()
        {
            var globalSettings = _globalSettingsQuery.Execute();

            return Json(globalSettings, JsonRequestBehavior.AllowGet);
        }

//        [RequireGlobalAdmin]
        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(GlobalSettingsViewModel model)
        {
            _updateGlobalSettingsCommand.Execute(model);
            return Json("Ok");
        }

	    public ActionResult PrinterTest()
	    {
		    return View();
	    }

	}
}