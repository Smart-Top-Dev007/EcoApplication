namespace EcoCentre.Models.Domain.Municipalities.Events
{
    public class MunicipalityUpdatedEvent
    {
        public MunicipalityUpdatedEvent(Municipality municipality)
        {
            MunicipalityId = municipality.Id;
            Name = municipality.Name;
            IsActive = municipality.Enabled;
        }

        public string MunicipalityId { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
    }
}