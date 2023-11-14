using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.Payments
{
	public class UserInfo
	{
		public UserInfo(User.User user)
		{
			Name = user.Name;
			Id = user.Id;
			Login = user.Login;
		}

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		public string Name { get; set; }
		public string Login { get; set; }
	}
}