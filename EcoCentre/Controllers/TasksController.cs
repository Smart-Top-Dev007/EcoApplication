using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using EcoCentre.Models;
using EcoCentre.Models.Commands.Scheduler;

namespace EcoCentre.Controllers
{
    using System;

    public class TasksController : AsyncController
    {
		private readonly IEnumerable<IAdminTask> _backgroundTasks;

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

		public TasksController(IEnumerable<IAdminTask> backgroundTasks)
        {
            _backgroundTasks = backgroundTasks;
        }

        public ActionResult Index()
        {
        	return PartialView("indexTemplate",_backgroundTasks);
            //return Json(_backgroundTasks, JsonRequestBehavior.AllowGet);
        }

		public ActionResult Run(int id)
		{
			var taks = _backgroundTasks.FirstOrDefault(x => x.Id == id);
            taks.Execute();
		    
			return RedirectToAction("Index");
			//return Json(_backgroundTasks, JsonRequestBehavior.AllowGet);
		}
        public ActionResult IndexTemplate()
        {
            return View();
        }

		[HttpPost]
	    public ActionResult SendEmail(string destinationAddress)
	    {
		    MailMessage mail = new MailMessage("ecocentre@sphyr.com", destinationAddress);
		    SmtpClient client = new SmtpClient();
		    mail.Subject = "this is a test email.";
		    mail.Body = "this is my test email body";
		    client.Send(mail);
			return RedirectToAction("Index");
	    }

	}
}
