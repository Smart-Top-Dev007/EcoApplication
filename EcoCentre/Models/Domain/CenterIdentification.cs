using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcoCentre.Models.Domain.Hubs;
using EcoCentre.Models.Domain.User;

namespace EcoCentre.Models.Domain
{
    public class CenterIdentification
    {
	    public string Name { get; set; }
		public string Url { get; set; }
	    public string Id { get; set; }
	    public string Address { get; set; }
    }
}