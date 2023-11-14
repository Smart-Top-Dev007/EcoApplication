using EcoCentre.Models.Domain;
using EcoCentre.Models.Domain.Reporting.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoCentre.Models.Queries
{
    public class CachedMaterialsByAddressQuery : Entity
    {
        public MaterialsByAddressReportQueryParams Query { get; set; }
        public List<string> CachedMaterialByAddressIds { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
