using System;
using System.Web;
using System.Web.Mvc;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Giveaway;

namespace EcoCentre.Controllers
{
	public class GiveawayController : Controller
	{
		private readonly FileRepository _fileRepository;
		private readonly SaveFileCommand _saveFileCommand;

		public GiveawayController(FileRepository fileRepository, SaveFileCommand saveFileCommand)
		{
			_fileRepository = fileRepository;
			_saveFileCommand = saveFileCommand;
		}

		public ActionResult IndexTemplate()
		{
			return View();
		}
		public ActionResult NewTemplate()
		{
			return View();
		}

		public ActionResult List(GiveawayQuery query)
		{
			var data = query.Execute();
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Index(GiveawayItemQuery query)
		{
			var data = query.Execute();
			if (data == null)
			{
				return HttpNotFound();
			}
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[RequireWriteRightsForPost]
		public ActionResult Index(CreateGiveawayCommand command)
		{
			command.Execute();
			return Json("OK", JsonRequestBehavior.AllowGet);
		}

		[HttpPut]
		[AcceptVerbs(HttpVerbs.Put)]
		[RequireWriteRightsForPost]
		public ActionResult Index(UpdateGiveawayCommand command)
		{
			command.Execute();
			return Json("OK", JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		[RequireWriteRightsForPost]
		public ActionResult Delete(DeleteGiveawayCommand command)
		{
			command.Execute();
			return Json("OK", JsonRequestBehavior.AllowGet);
		}

		[AllowAnonymous]
		public ActionResult Public(PublicGiveawayQuery query)
		{
			var data = query.Execute();
			return View(data);
		}

		[AllowAnonymous]
		public ActionResult Image(string id)
		{
			if (!String.IsNullOrEmpty(Request.Headers["If-Modified-Since"]))
			{
				Response.StatusCode = 304;
				Response.StatusDescription = "Not Modified";
				return Content(String.Empty);
			}

			var file = _fileRepository.Find(id);
			if (file == null)
				throw new HttpException(404, "File not found");
			
			Response.Cache.SetCacheability(HttpCacheability.Public);
			Response.Cache.SetLastModified(DateTime.UtcNow);
			return File(_fileRepository.GetBytes(id), "image/jpeg", file.Filename);
		}


		[RequireWriteRightsForPost]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Image(HttpPostedFileBase file)
		{
			var id = _saveFileCommand.Execute(file);

			var result = Json(id);
			result.ContentType = "text/plain";
			return result;
		}

		[RequireWriteRightsForPost]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult SetPublishingStatus(SetPublishingStatusCommand command)
		{
			command.Execute();
			return Json("OK", JsonRequestBehavior.AllowGet);
		}

	}
}
