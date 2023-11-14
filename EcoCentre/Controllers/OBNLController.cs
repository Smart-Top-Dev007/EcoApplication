using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoCentre.Models.Domain.Reporting.Journal;
using EcoCentre.Models.Domain.Reporting.Materials;
using EcoCentre.Models.Queries;

namespace EcoCentre.Controllers
{
    public class OBNLController
    {
          private readonly JournalReportQuery _journalReportQuery;
        private readonly JurnalReportXlsFormatter _jurnalReportXlsFormatter;
        private readonly ClientLimitsReportQuery _clientLimitsReportQuery;
        private readonly ClientVisitsLimitsReportQuery _clientVisitsLimitsReportQuery;
        private readonly MaterialsBroughtQuery _materialsBroughtQuery;
        private readonly MaterialBroughtXlsFormatter _broughtXlsFormatter;
        private readonly MaterialsByAddressReportQuery _materialByAddressQuery;
        private readonly MaterialByAddressXlsFormatter _materialByAddressXlsFormatter;

      /*  protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
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
        */
        public OBNLController(JournalReportQuery journalReportQuery, JurnalReportXlsFormatter jurnalReportXlsFormatter, ClientLimitsReportQuery clientLimitsReportQuery,
            MaterialsBroughtQuery materialsBroughtQuery, MaterialBroughtXlsFormatter broughtXlsFormatter, MaterialsByAddressReportQuery materialByAddressQuery, 
            MaterialByAddressXlsFormatter materialByAddressXlsFormatter, ClientVisitsLimitsReportQuery clientVisitsLimitsReportQuery)
        {
            _journalReportQuery = journalReportQuery;
            _jurnalReportXlsFormatter = jurnalReportXlsFormatter;
            _clientLimitsReportQuery = clientLimitsReportQuery;
            _materialsBroughtQuery = materialsBroughtQuery;
            _broughtXlsFormatter = broughtXlsFormatter;
            _materialByAddressQuery = materialByAddressQuery;
            _materialByAddressXlsFormatter = materialByAddressXlsFormatter;
            _clientVisitsLimitsReportQuery = clientVisitsLimitsReportQuery;
        }







    }
}