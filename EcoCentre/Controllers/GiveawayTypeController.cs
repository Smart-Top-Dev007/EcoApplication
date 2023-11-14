using System.Web.Mvc;
using EcoCentre.Models.Domain.Giveaway.Type;
using EcoCentre.Models.Infrastructure.Serialization;

namespace EcoCentre.Controllers
{
	public class GiveawayTypeController : Controller
	{
		public ActionResult IndexTemplate()
		{
			return View();
		}
		
		public ActionResult List(GiveawayTypeQuery query)
		{
			var data = query.Execute();
			return new CamelCaseJsonResult(data);
		}
		
		[HttpPost]
		[RequireWriteRightsForPost]
		public ActionResult Index(AddGiveawayTypeCommand command)
		{
			command.Execute();
			return new CamelCaseJsonResult("OK");
		}
		
		[HttpPost]
		[RequireWriteRightsForPost]
		public ActionResult Delete(DeleteGiveawayTypeCommand command)
		{
			command.Execute();
			return new CamelCaseJsonResult("OK");
		}
	}
}
