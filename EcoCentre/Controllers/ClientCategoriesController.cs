using System.Web.Mvc;
using EcoCentre.Models.Domain.Clients.Queries;

namespace EcoCentre.Controllers
{
    using System;

    public class ClientCategoriesController : Controller
    {
        private readonly ClientCategoriesDetailsQuery _clientCategoriesDetailsQuery;
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

        public ClientCategoriesController(ClientCategoriesDetailsQuery clientCategoriesDetailsQuery)
        {
            _clientCategoriesDetailsQuery = clientCategoriesDetailsQuery;
        }

        public ActionResult Index()
        {
            var data = _clientCategoriesDetailsQuery.Execute();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}