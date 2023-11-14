using System.Collections.Generic;
using System.Linq;

namespace EcoCentre.Models.Domain.Limits
{
    public class LimitStatusYear
    {
        public int Year { get; set; }

        public bool IsExceeding
        {
            get
            {
                var exceedingMats = Materials.Any(x => x.IsActive && x.MaxQuantity > 0 && x.QuantitySoFar > x.MaxQuantity);
                return exceedingMats;
            }
            set { }
        }

        public IList<LimitStatusMaterial> Materials { get; set; }
    }
}