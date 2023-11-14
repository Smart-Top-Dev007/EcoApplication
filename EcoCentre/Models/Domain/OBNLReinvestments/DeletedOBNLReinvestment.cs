using System;

namespace EcoCentre.Models.Domain.OBNLReinvestments
{
    public class DeletedOBNLReinvestment : Entity
    {
        public DateTime DeletedAt { get; set; }
        public OBNLReinvestment OBNLReinvestment { get; set; }
    }
}