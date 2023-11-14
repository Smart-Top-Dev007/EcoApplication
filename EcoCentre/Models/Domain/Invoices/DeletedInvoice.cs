using System;

namespace EcoCentre.Models.Domain.Invoices
{
    public class DeletedInvoice : Entity
    {
        public DateTime DeletedAt { get; set; }
        public Invoice Invoice { get; set; }
    }
}