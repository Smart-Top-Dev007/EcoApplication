using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.CompleteImport
{
	public class Municipality
	{
		[JsonProperty(PropertyName = "_id")]
		public string _id { get; set; }

		[JsonProperty(PropertyName = "Name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "NameLower")]
		public string NameLower { get; set; }

		[JsonProperty(PropertyName = "UpdatedAt")]
		public string UpdatedAt { get; set; }

		[JsonProperty(PropertyName = "CreatedAt")]
		public string CreatedAt { get; set; }

		[JsonProperty(PropertyName = "Enabled")]
		public bool Enabled { get; set; }

		[JsonProperty(PropertyName = "HubId")]
		public string HubId { get; set; }

		public Municipality()
		{
			_id = ObjectId.GenerateNewId().ToString();
			CreatedAt = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.032+0000'");
			UpdatedAt = DateTime.Now.AddDays(1).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.032+0000'");
			Enabled = true;
			HubId = null;
		}
	}
}