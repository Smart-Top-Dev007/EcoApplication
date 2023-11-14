using System.Web.Mvc;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Invoices.Commands;
using EcoCentre.Models.Domain.Invoices.Queries;
using EcoCentre.Models.Domain.OBNLReinvestments.Commands;
using EcoCentre.Models.Domain.OBNLReinvestments.Queries;
using EcoCentre.Models.Queries;

namespace EcoCentre.Controllers
{
    using System;

    public class OBNLReinvestmentController : Controller
    {
        private readonly CreateOBNLReinvestmentCommand _createInvoiceCommand;
        private readonly OBNLReinvestmentListQuery _invoiceListQuery;
        private readonly OBNLReinvestmentDetailsQuery _invoiceDetailsQuery;
        private readonly DeleteOBNLReinvestmentCommand _deleteInvoiceCommand;
        private readonly UndeleteOBNLReinvestmentCommand _undeleteInvoiceCommand;

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

        public OBNLReinvestmentController(CreateOBNLReinvestmentCommand createInvoiceCommand,
            OBNLReinvestmentListQuery invoiceListQuery, OBNLReinvestmentDetailsQuery invoiceDetailsQuery,
            DeleteOBNLReinvestmentCommand deleteInvoiceCommand,
            UndeleteOBNLReinvestmentCommand undeleteInvoiceCommand)
        {
            _createInvoiceCommand = createInvoiceCommand;
            _invoiceListQuery = invoiceListQuery;
            _invoiceDetailsQuery = invoiceDetailsQuery;
            _deleteInvoiceCommand = deleteInvoiceCommand;
            _undeleteInvoiceCommand = undeleteInvoiceCommand;
        }

        public ActionResult NewTemplate()
        {
            return View();
        }
        public ActionResult IndexTemplate()
        {
            return View();
        }
        public ActionResult TrashTemplate()
        {
            return View();
        }
        public ActionResult ShowTemplate()
        {
            return View();
        }

        public ActionResult Index(OBNLReinvestmentListQueryParams param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                var result = _invoiceListQuery.Execute(param);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            var invoice = _invoiceDetailsQuery.Execute(param.Id);
            return Json(invoice, JsonRequestBehavior.AllowGet);
        }

        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Post)][HttpPost]
        public ActionResult Index(OBNLReinvestmentViewModel param)
        {
            var result = _createInvoiceCommand.Execute(param);
            return Json(result);
        }

        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Post)][HttpPost]
        public ActionResult Undelete(string id)
        {
            var result = _undeleteInvoiceCommand.Execute(id);
            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult Index(string id)
        {
            var result = _deleteInvoiceCommand.Execute(id);
            return Json(result);
        }
    }


}
