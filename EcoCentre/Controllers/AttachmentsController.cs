using System.Web;
using System.Web.Mvc;
using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Common.Commands;
using EcoCentre.Models.Domain.Invoices.Commands;

namespace EcoCentre.Controllers
{
    public class AttachmentsController : Controller
    {
        private readonly SaveTmpFileCommand _saveTmpFileCommand;
        private readonly FileRepository _fileRepository;

        public AttachmentsController(SaveTmpFileCommand saveTmpFileCommand, FileRepository fileRepository)
        {
            _saveTmpFileCommand = saveTmpFileCommand;
            _fileRepository = fileRepository;
        }

        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(HttpPostedFileBase file)
        {
            var id = _saveTmpFileCommand.Execute(file);
            
            var result = Json(id);
            result.ContentType = "text/plain";
            return result;
        }

        public ActionResult Index(string id)
        {
            var file = _fileRepository.Find(id);
            if(file == null)
                throw new HttpException(404,"File not found");
            return File(_fileRepository.GetBytes(id), "image/jpeg", file.Filename);
        }

    }
}
