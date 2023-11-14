using System.IO;
using System.Linq;
using System.Web.Mvc;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Payments;
using EcoCentre.Models.MoneticoPayments;
using static EcoCentre.Models.Infrastructure.Serialization.ControllerExtensions;

namespace EcoCentre.Controllers
{
	public class PaymentController : Controller
	{
		public ActionResult SettingsTemplate()
		{
			return View();
		}

		[HttpGet]
		public ActionResult PayWithCreditCardRedirect(RedirectCreditCardCommand command)
		{
			var result = command.Execute();
			return View(result);
		}

		[RequireGlobalAdmin]
		[HttpGet]
		public ActionResult Settings(Repository<MoneticoConfiguration> repository)
		{
			var result = repository.Query.SingleOrDefault();
			if (result == null)
			{
				result = new MoneticoConfiguration
				{
					Language = "FR",
					Currency = "CAD",
					Environment = MoneticoEnvironment.Production
				};
			}

			return CamelCaseJsonForAngular(result);
		}

		[RequireGlobalAdmin]
		[HttpPost]
		public ActionResult Settings(SavePaymentSettingsCommand command)
		{
			command.Execute();
			return CamelCaseJsonForAngular("OK");
		}

		[HttpPost]
		public ActionResult PayWithCash(PayWithCashCommand command)
		{
			var result = command.Execute();
			return CamelCaseJsonForAngular(result);
		}

		[HttpPost]
		public ActionResult PayWithCredit(PayWithCreditCommand command)
		{
			var result = command.Execute();
			return CamelCaseJsonForAngular(result);
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult AuthorizationCallback(PaymentResponseCommand command)
		{
			var body = GetBody();
			var result = command.Execute(body);
			return Content(result);
		}

		private string GetBody()
		{
			var inputStream = Request.InputStream;
			inputStream.Position = 0;
			using (var reader = new StreamReader(inputStream))
			{
				return reader.ReadToEnd();
			}
		}
	}
}
