using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.CompleteImport
{
	public class Hub
	{
		[JsonProperty(PropertyName = "_id")]
		public string _id { get; set; }

		[JsonProperty(PropertyName = "Name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "InvoiceIdentifier")]
		public string InvoiceIdentifier { get; set; }

		[JsonProperty(PropertyName = "DefaultGiveawayPrice")]
		public int DefaultGiveawayPrice { get; set; }

		[JsonProperty(PropertyName = "Address")]
		public string Address { get; set; }

		[JsonProperty(PropertyName = "EmailForLoginAlerts")]
		public string EmailForLoginAlerts { get; set; }

		[JsonProperty(PropertyName = "LastChange")]
		public string LastChange { get; set; }

		public Hub()
		{
			_id = "5d494aeb559ab418c85d56d0";
			Name = "Default";
			InvoiceIdentifier = null;
			DefaultGiveawayPrice = 0;
			Address = null;
			EmailForLoginAlerts = null;
			LastChange = null;
		}
	}
}