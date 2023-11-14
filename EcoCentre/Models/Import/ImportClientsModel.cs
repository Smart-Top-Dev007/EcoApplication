using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoCentre.Models.Domain.Hubs;

namespace EcoCentre.Models.Import
{
    public class ImportClientsModel
    {
        public string HubId { get; set; }
        public IEnumerable<SelectListItem> Hubs { get; set; }
    }
}