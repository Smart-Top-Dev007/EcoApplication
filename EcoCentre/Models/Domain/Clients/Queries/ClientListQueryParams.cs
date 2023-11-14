using System;
using EcoCentre.Models.Domain.Common;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Queries;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace EcoCentre.Models.Domain.Clients.Queries
{
    public class ClientListQueryParams
    {
        public string Id { get; set; }
        public int Page { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string CivicNumber { get; set; }
		public string Term { get; set; }
        public string OBNLNumber { get; set; }
        public ClientListCategoryFilter CategoryFilter { get; set; }
		public ClientListSearchBy SearchType { get; set; }
        public string FirstLetter { get; set; }
        public string HubId { get; set; }
        public bool Active { get; set; }
        public bool Inactive { get; set; }
        public DateTime? LastVisitFrom { get; set; }
        public DateTime? LastVisitTo { get; set; }
        public bool NoCommercial { get; set; }
        public SortDir SortDir { get; set; }
        public ClientListQuerySortBy SortBy { get; set; }
        
        public int? PageSize { get; set; }
        public string CitizenCard { get; set; }
		public string PostalCode { get; set; }
		public DateTime LastChange { get; set; }
		public bool Verified { get; set; }

	}
}