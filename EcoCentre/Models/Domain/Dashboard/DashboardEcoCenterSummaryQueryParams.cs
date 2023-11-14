using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.Domain.Dashboard
{
    public class DashboardEcoCenterSummaryQueryParams
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}