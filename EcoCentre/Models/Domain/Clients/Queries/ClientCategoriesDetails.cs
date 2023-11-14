namespace EcoCentre.Models.Domain.Clients.Queries
{
	public class ClientCategoriesDetails
	{
		public ClientCategoriesDetails(ClientCategory cat, string text)
		{
			Category = cat;
			CategoryName = cat.ToString();
			Text = text;
		}
		public ClientCategory Category { get; set; }
		public string CategoryName { get; set; }
		public string Text { get; set; }
		public bool ShowFirstNameLastName { get; set; }
		public bool ShowOrganizationName { get; set; }
        public bool ShowOBNLNumber { get; set; }
	}
}