using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcoCentre.Models.Domain.OBNLReinvestments;

namespace EcoCentre.Models.Domain.Reporting.OBNL
{
    public class OBNLGlobal : Entity
    {
        public string ClientId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }

        public IQueryable<OBNLReinvestment> OBNLReinvestments { get; set; }
        public IQueryable<OBNLReinvestment> IncludedOBNLReinvestments { get; set; }
        public IEnumerable<OBNLGlobalMaterial> Materials { get; set; }
    }

    public class OBNLGlobalMaterial
    {
        public string Name { get; set; }
        public double Weight { get; set; }
    }
}