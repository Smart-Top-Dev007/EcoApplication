using EcoCentre.Models.Domain.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoCentre.Models.Domain.Invoices.Queries
{
    public class CompleteInvoicesListQueryParams
    {
        public string ClientId { get; set; }
        public Client Client { get; set; } // will only be used internally
    }
}