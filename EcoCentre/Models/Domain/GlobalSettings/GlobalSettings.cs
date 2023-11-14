using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.Domain.GlobalSettings
{
	[BsonIgnoreExtraElements]
	public class GlobalSettings : Entity
    {
        public GlobalSettings()
        {
            MaxYearlyClientVisits = 0;
            MaxYearlyClientVisitsWarning = 0;
            AdminNotificationsEmail = "";
	        ContainerFullNotificationsEmail = "";
	        QstTaxRate = 0;
	        GstTaxRate = 0;
	        SessionTimeoutInMinutes = null;
        }

	    public int? SessionTimeoutInMinutes { get; set; }

	    public int MaxYearlyClientVisits { get; set; }
        public int MaxYearlyClientVisitsWarning { get; set; }
        public string AdminNotificationsEmail { get; set; }
        public string ContainerFullNotificationsEmail { get; set; }
        public decimal GstTaxRate { get; set; }
        public decimal QstTaxRate { get; set; }
	    public string DefaultMaterialUnit { get; set; }
	    public string GstTaxLine { get; set; }
	    public string QstTaxLine { get; set; }
    }
}