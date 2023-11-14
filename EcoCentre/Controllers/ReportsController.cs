using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoCentre.Models.Domain.Clients.Queries;
using EcoCentre.Models.Domain.Reporting.Journal;
using EcoCentre.Models.Domain.Reporting.Materials;
using EcoCentre.Models.Infrastructure.Serialization;
using EcoCentre.Models.Queries;

namespace EcoCentre.Controllers
{
    public class ReportsController : Controller
    {
        private readonly JournalReportQuery _journalReportQuery;
        private readonly JurnalReportXlsFormatter _jurnalReportXlsFormatter;
        private readonly ClientLimitsReportQuery _clientLimitsReportQuery;
        private readonly ClientVisitsLimitsReportQuery _clientVisitsLimitsReportQuery;
        private readonly MaterialsBroughtQuery _materialsBroughtQuery;
        private readonly MaterialBroughtXlsFormatter _broughtXlsFormatter;
        private readonly MaterialsByAddressReportQuery _materialByAddressQuery;
        private readonly MaterialByAddressXlsFormatter _materialByAddressXlsFormatter;
        private readonly OBNLTotalReportQuery _obnlTotalReportQuery;
        private readonly OBNLTotalXlsFormatter _obnlTotalXlsFormatter;
        private readonly OBNLGlobalXlsFormatter _obnlGlobalXlsFormatter;
        private readonly OBNLGlobalReportQuery _obnlGlobalReportQuery;

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

        public ReportsController(JournalReportQuery journalReportQuery,
            JurnalReportXlsFormatter jurnalReportXlsFormatter, 
            ClientLimitsReportQuery clientLimitsReportQuery,
            MaterialsBroughtQuery materialsBroughtQuery, 
            MaterialBroughtXlsFormatter broughtXlsFormatter, 
            MaterialsByAddressReportQuery materialByAddressQuery, 
            MaterialByAddressXlsFormatter materialByAddressXlsFormatter, 
            ClientVisitsLimitsReportQuery clientVisitsLimitsReportQuery, 
            OBNLTotalReportQuery obnlTotalReportQuery,
            OBNLTotalXlsFormatter obnlTotalXlsFormatter,
            OBNLGlobalXlsFormatter obnlGlobalXlsFormatter,
            OBNLGlobalReportQuery obnlGlobalReportQuery)
        {
            _journalReportQuery = journalReportQuery;
            _jurnalReportXlsFormatter = jurnalReportXlsFormatter;
            _clientLimitsReportQuery = clientLimitsReportQuery;
            _materialsBroughtQuery = materialsBroughtQuery;
            _broughtXlsFormatter = broughtXlsFormatter;
            _materialByAddressQuery = materialByAddressQuery;
            _materialByAddressXlsFormatter = materialByAddressXlsFormatter;
            _clientVisitsLimitsReportQuery = clientVisitsLimitsReportQuery;
            _obnlTotalReportQuery = obnlTotalReportQuery;
            _obnlTotalXlsFormatter = obnlTotalXlsFormatter;
            _obnlGlobalXlsFormatter = obnlGlobalXlsFormatter;
            _obnlGlobalReportQuery = obnlGlobalReportQuery;
        }

        public ActionResult Journal(JournalReportQueryParams @params)
        {
            var result = _journalReportQuery.Execute(@params);
            if(!@params.Xls)
                return Json(result,JsonRequestBehavior.AllowGet);
            var xls = _jurnalReportXlsFormatter.Generate(result.Report);
            return File(xls, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "invoices.xlsx");
        }

        //
        // GET: /Reports/

        public ActionResult JournalTemplate()
        {
            return View();
        }

        public ActionResult Limits(ClientLimitsReportQueryParams param)
        {
            var result = _clientLimitsReportQuery.Execute(param);
            return Json(result,JsonRequestBehavior.AllowGet);
        }
        public ActionResult LimitsTemplate()
        {
            return View();
        }
        public ActionResult VisitsLimits(ClientLimitsReportQueryParams param)
        {
            var result = _clientVisitsLimitsReportQuery.Execute(param);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult VisitsLimitsTemplate()
        {
            return View();
        }

        public ActionResult Materials(MaterialsBroughtQueryParams param)
        {
            var result = _materialsBroughtQuery.Execute(param);
	        if (!param.Xls)
	        {
		        return new CamelCaseJsonResult(result);
	        }
			var xls = _broughtXlsFormatter.Generate(result);
            return File(xls, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Matériaux.xlsx");
        }
        public ActionResult MaterialsTemplate()
        {
            return View();
        }
        public ActionResult MaterialsByAddress(MaterialsByAddressReportQueryParams param)
        {
            if (!param.Xls)
                return Json(_materialByAddressQuery.Execute(param), JsonRequestBehavior.AllowGet);

            var xls = _materialByAddressXlsFormatter.Generate(_materialByAddressQuery.ExecuteAll(param));
            return File(xls, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Matériaux_par_adresse.xlsx");
        }
        public ActionResult MaterialsByAddressTemplate()
        {
            return View();
        }

        public ActionResult ObnlTotal(OBNLTotalReportParam param)
        {
            if (!param.Xls)
            {
                return Json(_obnlTotalReportQuery.Execute(param), JsonRequestBehavior.AllowGet);
            }

            var xls = _obnlTotalXlsFormatter.Generate(_obnlTotalReportQuery.Execute(param));
            return File(xls, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OBNL_totale.xlsx");
        }
        public ActionResult ObnlTotalTemplate()
        {
            return View();
        }

        public ActionResult ObnlGlobal(OBNLGlobalReportParam param)
        {
            if (!param.Xls)
            {
                return Json(_obnlGlobalReportQuery.ExecuteAll(param), JsonRequestBehavior.AllowGet);
            }

            var xls = _obnlGlobalXlsFormatter.Generate(_obnlGlobalReportQuery.ExecuteAll(param));
            return File(xls, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OBNL_matériaux_détails.xlsx");
        }
        public ActionResult ObnlGlobalTemplate()
        {
            return View();
        }

    }
}
