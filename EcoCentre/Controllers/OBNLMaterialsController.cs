using EcoCentre.Models.Domain.OBNLReinvestments.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcoCentre.Controllers
{
    public class OBNLMaterialsController : Controller
    {
        private readonly OBNLMaterialsQuery _obnlMaterialsQuery;

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

        public OBNLMaterialsController(OBNLMaterialsQuery obnlMaterialsQuery)
        {
            _obnlMaterialsQuery = obnlMaterialsQuery;
        }

        public ActionResult Index(OBNLMaterialsQueryParams param)
        {
            var result = _obnlMaterialsQuery.Execute(param);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
