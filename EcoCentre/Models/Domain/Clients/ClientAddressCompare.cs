using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.Domain.Clients
{
    public class ClientAddressStreetComparer: IEqualityComparer<ClientAddress>
    {
        public bool Equals(ClientAddress x, ClientAddress y)
        {
            return x.StreetLower == y.StreetLower;
        }

        public int GetHashCode(ClientAddress obj)
        {
            return obj.StreetLower.GetHashCode();
        }
    }
}