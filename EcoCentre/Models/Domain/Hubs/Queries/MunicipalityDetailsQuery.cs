namespace EcoCentre.Models.Domain.Hubs.Queries
{
    public class HubDetailsQuery
    {
        private readonly Repository<Hub> _hubRepository;

        public string Id { get; set; }
        public HubDetailsQuery(Repository<Hub> hubRepository)
        {
            _hubRepository = hubRepository;
        }

        public Hub Execute()
        {
            return _hubRepository.FindOne(Id);
        } 
    }
}