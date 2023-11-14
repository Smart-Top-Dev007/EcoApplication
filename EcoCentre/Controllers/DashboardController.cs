using System.Web.Mvc;
using EcoCentre.Models.Domain.Dashboard;
using EcoCentre.Models.Queries;

namespace EcoCentre.Controllers
{
    using System;

    public class DashboardController: Controller
    {
        private readonly DashboardDetailsQuery _dashboardDetailsQuery;

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

        public DashboardController(DashboardDetailsQuery dashboardDetailsQuery)
        {
            _dashboardDetailsQuery = dashboardDetailsQuery;
        }

        public ActionResult Index(DashboardEcoCenterSummaryQueryParams param)
        {
            var data = _dashboardDetailsQuery.Execute(param);
            return Json(data,JsonRequestBehavior.AllowGet);
        }
    }
}