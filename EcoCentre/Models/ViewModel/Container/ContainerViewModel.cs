using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.ViewModel.Container
{
	public class ContainerViewModel
	{
		public string Id{ get; set; }
		public string Number { get; set; }
		public decimal Capacity { get; set; }
		public decimal FillAmount { get; set; }
		public decimal AlertAtAmount { get; set; }
		public DateTime DateAdded { get; set; }
		public DateTime? DateOfLastAlert { get; set; }
		public string HubName { get; set; }
		public string HubId { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime? DateDeleted { get; set; }
		public List<MaterialViewModel> Materials { get; set; }
	}

	public class MaterialViewModel{
		public string Id { get; set; }
		public string Name { get; set; }
	}
}