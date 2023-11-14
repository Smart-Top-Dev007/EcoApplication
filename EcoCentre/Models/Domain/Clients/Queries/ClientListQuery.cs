using System;
using System.Linq;
using System.Text.RegularExpressions;
using EcoCentre.Models.Domain.Invoices;
using EcoCentre.Models.ViewModel;
using EcoCentre.Models.Domain.Invoices.Queries;
using EcoCentre.Models.Domain.OBNLReinvestments.Queries;
using EcoCentre.Models.Domain.User;
using EcoCentre.Models.Infrastructure;
using EcoCentre.Models.Queries;

namespace EcoCentre.Models.Domain.Clients.Queries
{
    public class ClientListQuery
    {
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<Invoice> _invoiceRepository;

        private readonly CompleteInvoicesListQuery _completeInvoicesListQuery;
        private readonly CompleteOBNLReinvestmentsListQuery _completeObnlReinvestmentsListQuery;
	    private readonly AuthenticationContext _context;

	    private const int PageSize = 20;
        public ClientListQuery(Repository<Client> clientRepository, 
            Repository<Invoice> invoiceRepository, 
            CompleteInvoicesListQuery completeInvoicesListQuery,
            CompleteOBNLReinvestmentsListQuery completeObnlReinvestmentsListQuery,
			AuthenticationContext context)
        {
            _clientRepository = clientRepository;
            _invoiceRepository = invoiceRepository;
            _completeInvoicesListQuery = completeInvoicesListQuery;
            _completeObnlReinvestmentsListQuery = completeObnlReinvestmentsListQuery;
	        _context = context;
        }

        public ClientListQueryResult Execute(ClientListQueryParams @params)
        {
            if (@params.Page < 1) @params.Page = 1;
            var skip = (@params.Page - 1) * (@params.PageSize ?? PageSize);
            var pageSize = @params.PageSize ?? PageSize;
            var query = _clientRepository.Query;

            var isTermSearch = false;
         /*   if (!string.IsNullOrEmpty(@params.OBNLNumber))
            {
                @params.SearchType = ClientListSearchBy.OBNLNumber;
            }*/
            if (!string.IsNullOrEmpty(@params.CitizenCard)) {
                query = query.Where(x => x.CitizenCard.Equals(@params.CitizenCard));
            }
			else if (@params.SearchType == ClientListSearchBy.Name)
			{
				if (!string.IsNullOrEmpty(@params.FirstName))
				{
                    @params.FirstName = @params.FirstName.Trim().ToLower();
					query = query.Where(x => x.FirstNameLower.Contains(@params.FirstName.ToLower()));
					isTermSearch = true;
				}
				if (!string.IsNullOrEmpty(@params.LastName))
				{
                    @params.LastName = @params.LastName.Trim().ToLower();
					query = query.Where(x => x.LastNameLower.Contains(@params.LastName));
					isTermSearch = true;
				}
			}

			else if (@params.SearchType == ClientListSearchBy.Address && !string.IsNullOrEmpty(@params.Address) && string.IsNullOrEmpty(@params.CivicNumber))
			{
                @params.Address = @params.Address.Trim().ToLower();
                //var tokens = @params.Address.Split(' ').Select(x => x.Trim()).Where(x => x.Length > 0).ToList();
				
                //var tokenQueries = tokens.Select(token => Query<Municipality>.Matches(x => x.NameLower, token)).ToList();
                //var qb = new QueryBuilder<Municipality>().Or(tokenQueries);
                //var cities = _municipalityRepository.Collection.Find(qb).Select(x=>x.Id).ToList();
                //query =
                //    query.Where(
                //        x =>
                //        x.Address.StreetLower.Contains(@params.Address) || x.Address.PostalCode.Contains(@params.Address) ||
                //        cities.Contains(x.Address.CityId));
                var addressRegExp = "(\\s|^)" + Regex.Escape(@params.Address);
                query = query.Where(x => Regex.IsMatch(x.Address.StreetLower, addressRegExp));
			}
            else if (@params.SearchType == ClientListSearchBy.Address && string.IsNullOrEmpty(@params.Address) && !string.IsNullOrEmpty(@params.CivicNumber))
            {
                @params.CivicNumber = @params.CivicNumber.Trim().ToUpper();
                query = query.Where(x => x.Address.CivicNumber.Equals(@params.CivicNumber));
            }
            else if (@params.SearchType == ClientListSearchBy.Address && !string.IsNullOrEmpty(@params.Address) && !string.IsNullOrEmpty(@params.CivicNumber))
			{
                @params.Address = @params.Address.Trim().ToLower();
                @params.CivicNumber = @params.CivicNumber.Trim().ToUpper();
                //var tokens = @params.Address.Split(' ').Select(x => x.Trim()).Where(x => x.Length > 0).ToList();
				
                //var tokenQueries = tokens.Select(token => Query<Municipality>.Matches(x => x.NameLower, token)).ToList();
                //var qb = new QueryBuilder<Municipality>().Or(tokenQueries);
                //var cities = _municipalityRepository.Collection.Find(qb).Select(x=>x.Id).ToList();
                //query =
                //    query.Where(
                //        x =>
                //        (x.Address.StreetLower.Contains(@params.Address) || x.Address.PostalCode.Contains(@params.Address) ||
                //        cities.Contains(x.Address.CityId)) && x.Address.CivicNumber.Equals(@params.CivicNumber));
                var addressRegExp = "(\\s|^)" + Regex.Escape(@params.Address);
                query = query.Where(x => Regex.IsMatch(x.Address.StreetLower, addressRegExp) && x.Address.CivicNumber.Equals(@params.CivicNumber));
			}
            else if(@params.SearchType==ClientListSearchBy.Category && !string.IsNullOrEmpty(@params.Term))
			{
                @params.Term = @params.Term.Trim().ToLower();
				query = query.Where(x => x.Category.Contains(@params.Term));
            }
			else if (@params.SearchType == ClientListSearchBy.Address && !string.IsNullOrEmpty(@params.PostalCode))
			{
				@params.PostalCode = @params.PostalCode.Trim();
				query = query.Where(x => x.Address.PostalCode.Contains(@params.PostalCode));
			}
			/*else if (@params.SearchType == ClientListSearchBy.Address && !string.IsNullOrEmpty(@params.Verified.ToString()))
			{
				var list = query.ToList();
				@params.PostalCode = @params.PostalCode.Trim();
				query = query.Where(x => x.Address.PostalCode.Contains(@params.PostalCode));
				list = query.ToList();
			}*/
			else if (@params.SearchType == ClientListSearchBy.OBNLNumber)
            {
                IQueryable<Client> clients = _clientRepository.Query.Where(x => x.Category == "OBNL");
                string clientId = null;
                if (@params.OBNLNumber != null)
                {
                    foreach (var client in clients)
                    {
                        if (client.OBNLNumbers != null && client.OBNLNumbers.IndexOf(@params.OBNLNumber) != -1)
                        {
                            clientId = client.Id;
                            break;
                        }
                    }
                    query = query.Where(x => x.Id == clientId);
                }
                else
                {
                    query = clients;
                }
            }
            if (@params.SearchType != ClientListSearchBy.OBNLNumber)
            {
                if (@params.CategoryFilter == ClientListCategoryFilter.OBNL)
                {
                    query = query.Where(q => q.Category == "OBNL");
                }
            }

            if (!isTermSearch && !string.IsNullOrEmpty(@params.FirstLetter))
            {
                @params.FirstLetter = @params.FirstLetter.ToLower();
                query = query.Where(x => x.LastNameLower.StartsWith(@params.FirstLetter));
            }

            //if (!String.IsNullOrEmpty(@params.HubId))
            //{
            //    query = query.Where(x => x.Hub.Id.Equals(@params.HubId));
            //}

        	if (@params.NoCommercial)
                query = query.Where(x => x.Category != "commercial");

            if (@params.Inactive)
            {
                query = query.Where(x => x.Status == ClientStatus.Inactive);
            }
            else
            {
                if (@params.Active)
                    query = query.Where(x => x.Status == ClientStatus.Active);
            }
			if (@params.Verified)
			{
				query = query.Where(x => x.Verified == true);
				var list = query.ToList();
			}
			else
			{
				query = query.Where(x => x.Verified == false);
				var list = query.ToList();
			}
				
			if (@params.LastVisitFrom != null || @params.LastVisitTo != null)
            {
                var invoiceQuery = _invoiceRepository.Query;
                if (@params.LastVisitFrom != null)
                    invoiceQuery = invoiceQuery.Where(x => x.CreatedAt >= @params.LastVisitFrom.Value);
                if (@params.LastVisitTo != null)
                    invoiceQuery = invoiceQuery.Where(x => x.CreatedAt < @params.LastVisitTo.Value);
                var clientIds = invoiceQuery.Select(x => x.ClientId).ToList().Distinct().ToList();
                query = query.Where(x => clientIds.Contains(x.Id));
            }
            var total = query.Count();

            switch (@params.SortBy)
            {
                case ClientListQuerySortBy.Category:
                    query = query.OrderBy(x => x.Category, @params.SortDir)
                                    .ThenBy(x => x.LastNameLower).ThenBy(x => x.FirstNameLower);
                    break;
                case ClientListQuerySortBy.LastName:
                    query = query.OrderBy(x => x.LastNameLower, @params.SortDir).ThenBy(x => x.FirstNameLower);
                    break;
                case ClientListQuerySortBy.FirstName:
                    query = query.OrderBy(x => x.FirstNameLower, @params.SortDir).ThenBy(x => x.LastNameLower);
                    break;
                case ClientListQuerySortBy.Email:
                    query = query.OrderBy(x => x.Email, @params.SortDir)
                                    .ThenBy(x => x.LastNameLower).ThenBy(x => x.FirstNameLower);
                    break;
                case ClientListQuerySortBy.PhoneNumber:
                    query = query.OrderBy(x => x.PhoneNumber, @params.SortDir)
                                    .ThenBy(x => x.LastNameLower).ThenBy(x => x.FirstNameLower);
                    break;
                case ClientListQuerySortBy.Address:
                    query = query.OrderBy(x => x.Address.CivicNumber, @params.SortDir)
                                    .ThenBy(x => x.Address.Street)
                                    .ThenBy(x => x.LastNameLower).ThenBy(x => x.FirstNameLower);
                    break;
                case ClientListQuerySortBy.City:
                    query = query.OrderBy(x => x.Address.City, @params.SortDir)
                                    .ThenBy(x => x.LastNameLower).ThenBy(x => x.FirstNameLower);
                    break;
				case ClientListQuerySortBy.RefId:
                    query = query.OrderBy(x => x.RefId, @params.SortDir)
                                    .ThenBy(x => x.LastNameLower).ThenBy(x => x.FirstNameLower);
                    break;
                case ClientListQuerySortBy.PersonalVisitsLimit:
                    query = query.OrderBy(x => x.PersonalVisitsLimit, @params.SortDir)
                                    .ThenBy(x => x.LastNameLower).ThenBy(x => x.FirstNameLower);
                    break;
				case ClientListQuerySortBy.PosteCode:
					query = query.OrderBy(x => x.PostalCode, @params.SortDir)
									.ThenBy(x => x.LastNameLower).ThenBy(x => x.FirstNameLower);
					break;

			}
            if (pageSize > 0)
            {
                query = query.Skip(skip).Take(pageSize);
            }
            var data = query
				.AsEnumerable()
				.Select(CreateClientViewModel)
				.ToList();

            var pageCount = pageSize > 0 ? (int)Math.Ceiling(total/(double) pageSize) : 1;
            return new ClientListQueryResult {Clients = data, Total = total, Page = @params.Page,PageCount = pageCount};

        }

	    private ClientViewModel CreateClientViewModel(Client client)
	    {
		    var invoiceDetails = _completeInvoicesListQuery.Execute(new CompleteInvoicesListQueryParams {Client = client});

		    var @params = new CompleteOBNLReinvestmentsListQueryParams{Client = client};
		    var obnlReinvestmentDetails = _completeObnlReinvestmentsListQuery.Execute(@params);

		    var result = new ClientViewModel(client, invoiceDetails, obnlReinvestmentDetails);

		    result.IsRegisteredInCurrentHub = client.Hub?.Id == _context.Hub?.Id;

		    return result;
	    }
    }
}