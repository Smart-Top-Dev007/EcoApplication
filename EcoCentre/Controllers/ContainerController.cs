using System.Linq;
using System.Web.Mvc;
using EcoCentre.Models.Domain.Containers;
using EcoCentre.Models.Domain.User;
using static EcoCentre.Models.Infrastructure.Serialization.ControllerExtensions;

namespace EcoCentre.Controllers
{
	public class ContainerController : Controller
	{
		public ActionResult IndexTemplate()
		{
			return View();
		}
		public ActionResult NewTemplate()
		{
			return View();
		}


		public ActionResult Index(ContainerQuery query)
		{
			if (string.IsNullOrWhiteSpace(query.Id))
			{
				return HttpNotFound();
			}
			query.Page = 1;
			query.PageSize = 1;

			var data = query.Execute();

			var item = data.Items?.FirstOrDefault();
			if (item == null)
			{
				return HttpNotFound();
			}
			return CamelCaseJsonForAngular(item);
		}
		public ActionResult List(ContainerXlsFormatter containerXlsFormatter, ContainerQuery query, bool xls = false)
		{
			var data = query.Execute();
			
			if (xls)
			{
				var bytes = containerXlsFormatter.Generate(data);
				return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Conteneurs.xlsx");
			}

			return CamelCaseJsonForAngular(data);
		}

		[HttpPost]
		[RequireWriteRightsForPost]
		public ActionResult Index(CreateContainerCommand command)
		{
			command.Execute();
			return CamelCaseJsonForAngular("OK");
		}

		[HttpPut]
		[AcceptVerbs(HttpVerbs.Put)]
		[RequireWriteRightsForPost]
		public ActionResult Index(UpdateContainerCommand command)
		{
			command.Execute();
			return Json("OK");
		}

		[HttpPost]
		[RequireWriteRightsForPost]
		public ActionResult SendAlert(SendAlertCommand command, string id, AuthenticationContext context)
		{
			var result = command.Execute(id, context.User.Id);
			return CamelCaseJsonForAngular(result);
		}

		[HttpPost]
		[RequireWriteRightsForPost]
		public ActionResult Delete(DeleteContainerCommand command)
		{
			command.Execute();
			return Json("OK", JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[RequireWriteRightsForPost]
		public ActionResult Undelete(UndeleteContainerCommand command)
		{
			command.Execute();
			return Json("OK", JsonRequestBehavior.AllowGet);
		}
	}
}
