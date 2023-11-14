using System.Collections.Generic;

namespace EcoCentre.Models.Domain.OBNLReinvestments.Commands
{
    public class OBNLReinvestmentViewModel
    {

        public string ClientId { get; set; }
        public string EmployeeName { get; set; }
        public IList<OBNLReinvestmentMaterialViewModel> Materials { get; set; }
        public IList<OBNLReinvestmentAttachmentViewModel> Attachments { get; set; }
        public string Comment { get; set; }
    }
}