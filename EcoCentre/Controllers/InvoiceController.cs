using System.Linq;
using System.Web.Mvc;
using EcoCentre.Models.Domain.Invoices.Commands;
using EcoCentre.Models.Domain.Invoices.Queries;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Payments;
using EcoCentre.Models.Infrastructure;
using EcoCentre.Models.Infrastructure.Serialization;

namespace EcoCentre.Controllers
{
    using System;
	using System.Text;
	using System.Web.Script.Serialization;	

	public class InvoiceController : Controller
    {
        private readonly CreateInvoiceCommand _createInvoiceCommand;
        private readonly PreviewInvoiceCommand _previewInvoiceCommand;
        private readonly InvoiceListQuery _invoiceListQuery;
        private readonly InvoiceDetailsQuery _invoiceDetailsQuery;
        private readonly DeleteInvoiceCommand _deleteInvoiceCommand;
        private readonly UndeleteInvoiceCommand _undeleteInvoiceCommand;

		public static string public_BarCode = "";

		public static string Public_BarCode
		{
			get
			{
				return public_BarCode;
			}

			set
			{
				public_BarCode = value;
			}
		}

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

        public InvoiceController(CreateInvoiceCommand createInvoiceCommand,InvoiceListQuery invoiceListQuery, InvoiceDetailsQuery invoiceDetailsQuery, DeleteInvoiceCommand deleteInvoiceCommand,
            UndeleteInvoiceCommand undeleteInvoiceCommand, PreviewInvoiceCommand previewInvoiceCommand)
        {
            _createInvoiceCommand = createInvoiceCommand;
            _invoiceListQuery = invoiceListQuery;
            _invoiceDetailsQuery = invoiceDetailsQuery;
            _deleteInvoiceCommand = deleteInvoiceCommand;
            _undeleteInvoiceCommand = undeleteInvoiceCommand;
	        _previewInvoiceCommand = previewInvoiceCommand;
        }

        public ActionResult NewTemplate()
        {
            return View();
        }
		public ActionResult NewTemplate2023()
		{
			return View();
		}

		public ActionResult NewPocPageTemplate()
		{
			return View();
		}
		public ActionResult NewOBNLTemplate()
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

		[AllowAnonymous]
		public string ReceiveBarCode(BarCode barCode)
		{
			public_BarCode = barCode.text;
			return public_BarCode;
		}

		public ContentResult Get_BarCode()
		{
			var sb = new StringBuilder();		

			JavaScriptSerializer ser = new JavaScriptSerializer();

			var serializedObject = ser.Serialize(new { message = public_BarCode });

			sb.AppendFormat("data: {0}\n\n", serializedObject);			

			return Content(sb.ToString(), "text/event-stream");
		
		}

		public void Init_BarCode()
		{
			public_BarCode = "";
		}

		public ActionResult Index(InvoiceListQueryParams param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                var result = _invoiceListQuery.Execute(param);
                return this.CamelCaseJsonForAngular(result);
            }
            var invoice = _invoiceDetailsQuery.Execute(param.Id);
            return this.CamelCaseJsonForAngular(invoice);
        }

        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Post)][HttpPost]
        public ActionResult Index(InvoiceViewModel param)
        {
            var result = _createInvoiceCommand.Execute(param);
            return Json(result);
		}

		[RequireWriteRightsForPost]
		[AcceptVerbs(HttpVerbs.Post)]
		[HttpPost]
		public ActionResult New1(InvoiceViewModel param)
		{
			var result = _createInvoiceCommand.Execute(param);
			return Json(result);
		}

		[RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Post)][HttpPost]
        public ActionResult Preview(InvoiceViewModel param)
        {
	        var result = _previewInvoiceCommand.Execute(param);
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

		
	    public ActionResult _Header()
	    {
		    return PartialView();
	    }
	    public ActionResult PaymentTypes()
	    {
		    var values = Enum.GetValues(typeof(PaymentMethod)).Cast<PaymentMethod>()
			    .Select(x => new
			    {
				    Id = (int) x,
				    Value = x.ToString(),
				    Description = x.GetDescription()
			    });

			return new CamelCaseJsonResult(values);
	    }
	}


}
