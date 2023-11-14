using System.Web.Mvc;

namespace EcoCentre.Controllers
{
	public class TemplateController : Controller
    {
        public ActionResult Index(string folder, string file)
        {
	        return PartialView($"~/Scripts/ng/{folder}/{file}.template.cshtml");
        }
	}
}
