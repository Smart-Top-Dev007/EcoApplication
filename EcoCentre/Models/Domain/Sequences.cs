using System.Linq;
using MongoDB.Driver;

namespace EcoCentre.Models.Domain
{
    public class Sequences
    {
        private readonly Repository<Sequence> _sequencesRepository;

        public Sequences(Repository<Sequence> sequencesRepository)
        {
            _sequencesRepository = sequencesRepository;
        }
        public int GetNext(string name)
        {
            if (!_sequencesRepository.Query.Any(x => x.Name == name))
            {
                var newSeq = new Sequence
                    {
                        Name = name,
                        Counter = 1
                    };
                _sequencesRepository.Collection.InsertOne(newSeq);
            }

	        var cmd = _sequencesRepository.Collection.FindOneAndUpdate(
		        x => x.Name == name,
		        Builders<Sequence>.Update.Inc(x => x.Counter, 1)
	        );

            return cmd.Counter;
        }

        public int NextInvoice()
        {
            return GetNext("Invoice");
        }

        public int NextOBNLReinvestment()
        {
            return GetNext("OBNLReinvestment");
        }
    }
}