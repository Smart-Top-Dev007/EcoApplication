using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.Domain.Reporting.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcoCentre.Models.Domain.Common;

namespace EcoCentre.Models.Queries
{
    public class MaterialsByAddressReportQueryParams
    {
        public int Page { get; set; }
        public bool Xls { get; set; }
        public SortDir SortDir { get; set; }
        public MaterialByAddressSortBy SortBy { get; set; }
        public int SortIndex { get; set; }
        public string FilterFirstName { get; set; }
        public string FilterLastName { get; set; }
        public string FilterStreet { get; set; }
        public string FilterCivicNumber { get; set; }
        public string SearchTerm { get; set; }
        public MaterialByAddressSearchBy SearchType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string CenterName { get; set; }
        public int? PageSize { get; set; }
        public bool PersonalVisitsLimitHigherThenGlobalOnly { get; set; }

        public bool AllClients { get; set; }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            MaterialsByAddressReportQueryParams p = obj as MaterialsByAddressReportQueryParams;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            //return (x == p.x) && (y == p.y);
            if (p.SearchType == SearchType && p.CenterName == CenterName)
            {
                if (((p.FromDate != null && FromDate != null) ? (DateTime.Compare(p.FromDate.Value.ToUniversalTime().Date, FromDate.Value.ToUniversalTime().Date) == 0) : (p.FromDate == null && FromDate == null)) &&
                    ((p.ToDate != null && ToDate != null) ? (DateTime.Compare(p.ToDate.Value.ToUniversalTime().Date, ToDate.Value.ToUniversalTime().Date) == 0) : (p.ToDate == null && ToDate == null)))
                {
                    switch (SearchType)
                    {
                        case MaterialByAddressSearchBy.ClientName:
                            return p.FilterFirstName == FilterFirstName &&
                                    p.FilterLastName == FilterLastName;
                        case MaterialByAddressSearchBy.Address:
                            return p.FilterStreet == FilterStreet &&
                                    p.FilterCivicNumber == FilterCivicNumber;
                        case MaterialByAddressSearchBy.City:
                        case MaterialByAddressSearchBy.PostalCode:
                            return p.SearchTerm == SearchTerm;
                    }
                }

            }
            return false;
        }

        public bool Equals(MaterialsByAddressReportQueryParams p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            if (p.SearchType == SearchType && p.CenterName == CenterName)
            {
                if (((p.FromDate != null && FromDate != null) ? (DateTime.Compare(p.FromDate.Value.ToUniversalTime().Date, FromDate.Value.ToUniversalTime().Date) == 0) : (p.FromDate == null && FromDate == null)) &&
                    ((p.ToDate != null && ToDate != null) ? (DateTime.Compare(p.ToDate.Value.ToUniversalTime().Date, ToDate.Value.ToUniversalTime().Date) == 0) : (p.ToDate == null && ToDate == null)))
                {
                    switch (SearchType)
                    {
                        case MaterialByAddressSearchBy.ClientName:
                            return p.FilterFirstName == FilterFirstName &&
                                    p.FilterLastName == FilterLastName;
                        case MaterialByAddressSearchBy.Address:
                            return p.FilterStreet == FilterStreet &&
                                    p.FilterCivicNumber == FilterCivicNumber;
                        case MaterialByAddressSearchBy.City:
                        case MaterialByAddressSearchBy.PostalCode:
                            return p.SearchTerm == SearchTerm;
                    }
                }

            }
            return false;
        }

        public override int GetHashCode()
        {
            return (FilterFirstName ?? "").GetHashCode() ^
                   (FilterLastName ?? "").GetHashCode() ^
                   (FilterStreet ?? "").GetHashCode() ^
                   (FilterCivicNumber ?? "").GetHashCode() ^
                   (SearchTerm ?? "").GetHashCode() ^
                   SearchType.GetHashCode() ^
                   FromDate.GetHashCode() ^
                   ToDate.GetHashCode();
        }

        public static bool operator ==(MaterialsByAddressReportQueryParams a, MaterialsByAddressReportQueryParams b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            if (a.SearchType == b.SearchType && a.CenterName == b.CenterName)
            {
                if (((a.FromDate != null && b.FromDate != null) ? (DateTime.Compare(a.FromDate.Value.ToUniversalTime().Date, b.FromDate.Value.ToUniversalTime().Date) == 0) : (a.FromDate == null && b.FromDate == null)) &&
                    ((a.ToDate != null && b.ToDate != null) ? (DateTime.Compare(a.ToDate.Value.ToUniversalTime().Date, b.ToDate.Value.ToUniversalTime().Date) == 0) : (a.ToDate == null && b.ToDate == null)))
                {
                    switch (b.SearchType)
                    {
                        case MaterialByAddressSearchBy.ClientName:
                            return a.FilterFirstName == b.FilterFirstName &&
                                    a.FilterLastName == b.FilterLastName;
                        case MaterialByAddressSearchBy.Address:
                            return a.FilterStreet == b.FilterStreet &&
                                    a.FilterCivicNumber == b.FilterCivicNumber;
                        case MaterialByAddressSearchBy.City:
                        case MaterialByAddressSearchBy.PostalCode:
                            return a.SearchTerm == b.SearchTerm;
                    }
                }

            }
            return false;
        }

        public static bool operator !=(MaterialsByAddressReportQueryParams a, MaterialsByAddressReportQueryParams b)
        {
            return !(a == b);
        }
    }
}