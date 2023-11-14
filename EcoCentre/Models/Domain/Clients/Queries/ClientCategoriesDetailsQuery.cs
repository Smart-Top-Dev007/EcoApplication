using System.Net.Mime;

namespace EcoCentre.Models.Domain.Clients.Queries
{
	public class ClientCategoriesDetailsQuery
	{
		public ClientCategoriesDetails[] Execute()
		{
			return new[]
			{
				new ClientCategoriesDetails(ClientCategory.Resident, Resources.Model.Client.CategoryResident)
				{
					ShowFirstNameLastName = true,
				},
				new ClientCategoriesDetails(ClientCategory.Municipality, Resources.Model.Client.CategoryMunicipality)
				{
					ShowOrganizationName = true
				},
				new ClientCategoriesDetails(ClientCategory.Commerce, Resources.Model.Client.CategoryCommerce)
				{
					ShowOrganizationName = true
				},
				new ClientCategoriesDetails(ClientCategory.Institution, Resources.Model.Client.CategoryInstitution)
				{
					ShowOrganizationName = true
				},
				new ClientCategoriesDetails(ClientCategory.OBNL, Resources.Model.Client.CategoryOBNL)
				{
					ShowFirstNameLastName = true,
					ShowOBNLNumber = true
				},
				new ClientCategoriesDetails(ClientCategory.Other, Resources.Model.Client.CategoryOther)
				{
					ShowFirstNameLastName = true,
					ShowOrganizationName = true
				}
			};

		}
	}
}