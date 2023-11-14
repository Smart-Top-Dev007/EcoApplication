using System;
using System.Collections.Generic;
using EcoCentre.Models.Domain.Clients;

namespace EcoCentre.Models.Domain.Invoices.Queries
{
    public class InvoiceClientDetails
    {
        public InvoiceClientDetails(InvoiceDetails invoice)
        {
            Id = invoice.Id;
            InvoiceNo = invoice.InvoiceNo;
            Center = invoice.Center;
            IsExcluded = invoice.IsExcluded;
        }

        public CenterIdentification Center { get; set; }

        public string Id { get; set; }
        public string InvoiceNo { get; set; }

        public bool IsExcluded { get; set; }
    }
}