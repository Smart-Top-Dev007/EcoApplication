using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.Queries
{
    public class MaterialByAddressCachingSettings
    {
        public int MaxCachedRequestsNumber { get; set; }
        public int MaxCachedRequestAgeInDays { get; set; }
    }
}