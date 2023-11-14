using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcoCentre.Models.Domain.Giveaway;
using EcoCentre.Models.Domain.Hubs;

namespace EcoCentre.Models.ViewModel.Giveaway
{
	public class PublicListingViewModel
	{
		public List<GiveawayItem> Items { get; set; }
		public Hub SelectedHub { get; set; }
		public List<Hub> Hubs { get; set; }
	}
}