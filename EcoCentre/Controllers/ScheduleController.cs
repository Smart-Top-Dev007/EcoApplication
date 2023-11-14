using System;
using System.Web.Mvc;
using EcoCentre.Models.Commands.Scheduler;
using MassTransit;

namespace EcoCentre.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly RunScheduledTasksCommand _runScheduledTasksCommand;
    	private static bool _isExecuting;
        private static readonly object ExecutingLock = new object();

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

        public ScheduleController(RunScheduledTasksCommand runScheduledTasksCommand)
        {
        	_runScheduledTasksCommand = runScheduledTasksCommand;
        }

    	[AllowAnonymous]
        public ActionResult Index(string key)
        {
            if (key != "uDG81TICusSd") return Content("Error");
            try
            {
                lock (ExecutingLock)
                {
                    if (_isExecuting) return Content("");
                    _isExecuting = true;
                }
                _runScheduledTasksCommand.Execute();

            }
            finally
            {
                _isExecuting = false;
            }
            return Content("");
        }
    }

}