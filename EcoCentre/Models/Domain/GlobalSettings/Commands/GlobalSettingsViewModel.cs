using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.Domain.GlobalSettings.Commands
{
    public class GlobalSettingsViewModel
    {
        public int MaxYearlyClientVisits { get; set; }
        public int MaxYearlyClientVisitsWarning { get; set; }
        public string AdminNotificationsEmail { get; set; }
	    public decimal GstTaxRate { get; set; }
	    public string GstTaxLine { get; set; }
	    public decimal QstTaxRate { get; set; }
	    public string QstTaxLine { get; set; }
		public string DefaultMaterialUnit { get; set; }
	    public string ContainerFullNotificationsEmail { get; set; }
	    public int? SessionTimeoutInMinutes { get; set; }
    }
}