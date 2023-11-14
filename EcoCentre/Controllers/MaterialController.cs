using System.Web.Mvc;
using EcoCentre.Models.Commands;
using EcoCentre.Models.Domain.Materials.Commands;
using EcoCentre.Models.Queries;
using EcoCentre.Models.Domain.Clients.Commands;
using static EcoCentre.Models.Infrastructure.Serialization.ControllerExtensions;

namespace EcoCentre.Controllers
{
    using System;

    public class MaterialController : Controller
    {
        private readonly CreateMaterialCommand _createMaterialCommand;
        private readonly MaterialListQuery _materialListQuery;
        private readonly MaterialDetailsQuery _materialDetailsQuery;
        private readonly UpdateMaterialCommand _updateMaterialCommand;
        private readonly MergeMaterialsCommand _mergeMaterialsCommand;

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

        public MaterialController(CreateMaterialCommand createMaterialCommand, MaterialListQuery materialListQuery,
            MaterialDetailsQuery materialDetailsQuery, UpdateMaterialCommand updateMaterialCommand,
            MergeMaterialsCommand mergeMaterialsCommand)
        {
            _createMaterialCommand = createMaterialCommand;
            _materialListQuery = materialListQuery;
            _materialDetailsQuery = materialDetailsQuery;
            _updateMaterialCommand = updateMaterialCommand;
            _mergeMaterialsCommand = mergeMaterialsCommand;
        }

        public ActionResult IndexTemplate()
        {
            return View();
        }
     
        public ActionResult NewTemplate()
        {
            return View();
        }

        public ActionResult Index(MaterialListQueryParam param)
        {
            if (string.IsNullOrEmpty(param.Id))
            {
                var items = _materialListQuery.Execute(param);
                return CamelCaseJsonForAngular(items);
            }
            var item = _materialDetailsQuery.Execute(param.Id);
            return CamelCaseJsonForAngular(item);

        }

        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(MaterialViewModel material)
        {
            _createMaterialCommand.Execute(material);
            return Json("OK");
        }

        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Put)]
        public ActionResult Index(ExistingMaterialViewModel material)
        {
            _updateMaterialCommand.Execute(material);
            return Json("OK");
        }
        public ActionResult MergerTemplate()
        {
            return View();
        }
        [HandleError()]
        [RequireWriteRightsForPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Merge(MergeCommandParams @params)
        {
            var result = _mergeMaterialsCommand.Execute(@params);
            return Json(result);
        }

    }

}
