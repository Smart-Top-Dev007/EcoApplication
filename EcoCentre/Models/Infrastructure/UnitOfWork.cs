using System;
using MongoDB.Driver;
using System.Security.Authentication;

namespace EcoCentre.Models.Infrastructure
{
    public class UnitOfWork
    {
        public UnitOfWork(string connectionString)
        {
	        var mongoUrl = new MongoUrl(connectionString);
	        var settings = MongoClientSettings.FromUrl(mongoUrl);

	        // Connections on azure seem to have a problem.
	        // Occasionally they time out while sending data or trying to open.
	        // My theory is that connection in connection pool are disrupted
	        // after some time of inactivity somewhere between app and mongo.
	        // When this broken connection is picked up and reused, it's still considered
	        // to be ok from app's point of view. When app tries to use it, no one is
	        // listening on the other side.
	        // A fix that looks to help is to reduce lifetime of connection in pool.
	        // Other less aggressive options could be to reduce connection heartbeat timeout.
	        settings.MaxConnectionIdleTime = TimeSpan.FromMinutes(1);
	        settings.MaxConnectionLifeTime = TimeSpan.FromMinutes(2);

			settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };
				        
			var client = new MongoClient(settings);
			Database = client.GetDatabase(mongoUrl.DatabaseName);
        }
		
        public IMongoDatabase Database { get; }
    }
}