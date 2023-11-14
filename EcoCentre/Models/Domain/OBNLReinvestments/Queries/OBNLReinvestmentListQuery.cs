using System;
using System.Linq;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Common;
using EcoCentre.Models.Domain.User;
using EcoCentre.Models.Infrastructure;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EcoCentre.Models.Domain.OBNLReinvestments.Queries
{
	// ReSharper disable once InconsistentNaming
    public class OBNLReinvestmentListQuery
    {
        private readonly Repository<OBNLReinvestment> _invoiceRepository;
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<DeletedOBNLReinvestment> _deletedInvoiceRepository;
        private readonly CenterIdentification _centerIdentification;
        private readonly AuthenticationContext _authContext;

        private const int PageSize = 20;
        public OBNLReinvestmentListQuery(Repository<OBNLReinvestment> invoiceRepository, Repository<Client> clientRepository, 
            Repository<DeletedOBNLReinvestment> deletedInvoiceRepository, CenterIdentification centerIdentification,
            AuthenticationContext authContext)
        {
            _invoiceRepository = invoiceRepository;
            _clientRepository = clientRepository;
            _deletedInvoiceRepository = deletedInvoiceRepository;

            _centerIdentification = centerIdentification;
            _authContext = authContext;
        }

        public OBNLReinvestmentListQueryResult Execute(OBNLReinvestmentListQueryParams @params)
        {
            if (@params.Deleted)
                return DeletedInvoiceList(@params);
            return InvoiceList(@params);
        }

        private OBNLReinvestmentListQueryResult DeletedInvoiceList(OBNLReinvestmentListQueryParams @params)
        {
            if (@params.Page < 1) @params.Page = 1;
            var skip = (@params.Page - 1) * (@params.PageSize ?? PageSize);
            var pageSize = @params.PageSize ?? PageSize;
            var query = _deletedInvoiceRepository.Collection.AsQueryable();

            if (!_authContext.User.IsGlobalAdmin)
            {
                query = query.Where(x => x.OBNLReinvestment.Center != null && x.OBNLReinvestment.Center.Name == _centerIdentification.Name);
            }

            var total = query.Count();
            query = query.OrderByDescending(x => x.DeletedAt);

            if (pageSize > 0)
            {
                query = query.Skip(skip).Take(pageSize);
            }
            var invoices = query.ToList();
            var pageCount = pageSize > 0 ? (int)Math.Ceiling(total / (double)pageSize) : 1;

            var clientIds = invoices.Select(x => x.OBNLReinvestment.ClientId);
            var clients = _clientRepository.Collection.AsQueryable().Where(x => clientIds.Contains(x.Id)).ToList();
            var invoiceDetails = invoices.Select(i => new OBNLReinvestmentDetails(i.OBNLReinvestment, clients.FirstOrDefault(x => x.Id == i.OBNLReinvestment.ClientId)));

            return new OBNLReinvestmentListQueryResult
            {
                OBNLReinvestments = invoiceDetails,
                Total = total,
                Page = @params.Page,
                PageCount = pageCount
            };
        }

        private OBNLReinvestmentListQueryResult InvoiceList(OBNLReinvestmentListQueryParams @params)
        {
            if (@params.Page < 1) @params.Page = 1;
            var skip = (@params.Page - 1) * (@params.PageSize ?? PageSize);
            var pageSize = @params.PageSize ?? PageSize;
            var query = _invoiceRepository.Query;

            if (!_authContext.User.IsGlobalAdmin && !_authContext.User.IsReadOnly)
            {
                query = query.Where(x => x.Center != null && x.Center.Name == _centerIdentification.Name);
            }

            query = ApplySearchTerm(@params, query);

            if (!string.IsNullOrEmpty(@params.UserId) && @params.UserId.Length == 24)
                query = query.Where(x => x.ClientId == @params.UserId);

            var thisYear = new DateTime(DateTime.UtcNow.Year, 1, 1);
            query = @params.CurrentYear ? query.Where(x => x.CreatedAt >= thisYear) : query.Where(x => x.CreatedAt < thisYear);
            
            var total = query.Count();

            query = ApplySort(@params, query);

            if (pageSize > 0)
            {
                query = query.Skip(skip).Take(pageSize);
            }
            var invoices = query.ToList();
            var pageCount = pageSize > 0 ? (int) Math.Ceiling(total/(double) pageSize) : 1;

            var clientIds = invoices.Select(x => x.ClientId);
            var clients = _clientRepository.Collection.AsQueryable().Where(x => clientIds.Contains(x.Id)).ToList();
            var invoiceDetails = invoices.Select(i => new OBNLReinvestmentDetails(i, clients.FirstOrDefault(x => x.Id == i.ClientId)));

            return new OBNLReinvestmentListQueryResult
            {
                OBNLReinvestments = invoiceDetails,
                    Total = total,
                    Page = @params.Page,
                    PageCount = pageCount
                };

        }

        private static IQueryable<OBNLReinvestment> ApplySort(OBNLReinvestmentListQueryParams @params, IQueryable<OBNLReinvestment> query)
        {
            switch (@params.SortBy)
            {
                case OBNLReinvestmentSortTerm.OBNLReinvestmentDate:
                    query = query.OrderBy(x => x.CreatedAt, @params.SortDir);
                    break;
                case OBNLReinvestmentSortTerm.OBNLReinvestmentNo:
                    query = query.OrderBy(x => x.SequentialNo, @params.SortDir);
                    break;
            }
            return query;
        }

        private IQueryable<OBNLReinvestment> ApplySearchTerm(OBNLReinvestmentListQueryParams @params, IQueryable<OBNLReinvestment> query)
        {
            if (!string.IsNullOrEmpty(@params.Term)) @params.Term = @params.Term.Trim();
            if (_authContext.User.IsGlobalAdmin && !string.IsNullOrEmpty(@params.CenterName) && @params.CenterName.ToLower() != "tous")
            {
                query = query.Where(x => x.Center != null && x.Center.Name.Equals(@params.CenterName));
            }

            switch (@params.Type)
            {
                case OBNLReinvestmentSearchBy.Number:
                    if (string.IsNullOrEmpty(@params.Term)) break;
                    var tokens = @params.Term.Split('-');
                    if (tokens.Length == 1)
                        query = query.Where(x => x.SequentialNo == tokens[0].ToInt());
                    else if (tokens.Length == 2)
                        query = query.Where(x => x.SequentialNo == tokens[1].ToInt());
                    break;
                case OBNLReinvestmentSearchBy.ClientFirstName:
                    if (string.IsNullOrEmpty(@params.Term)) break;
                    var ci1 =
                        _clientRepository.Query.Where(x => x.FirstNameLower.Contains(@params.Term.ToLower()))
                                            .Select(x => x.Id)
                                            .ToList();
                    query = query.Where(x => ci1.Contains(x.ClientId));
                    break;
                case OBNLReinvestmentSearchBy.ClientLastName:
                    if (string.IsNullOrEmpty(@params.Term)) break;
                    var ci2 =
                        _clientRepository.Query.Where(x => x.LastNameLower.Contains(@params.Term.ToLower()))
                                            .Select(x => x.Id)
                                            .ToList();
                    query = query.Where(x => ci2.Contains(x.ClientId));
                    break;
                case OBNLReinvestmentSearchBy.ClientAddress:
                    if (string.IsNullOrEmpty(@params.Term)) break;
                    var ci3 =
                        _clientRepository.Query.Where(x => x.Address.City.Contains(@params.Term) || x.Address.StreetLower.Contains(@params.Term.ToLower()))
                                            .Select(x => x.Id)
                                            .ToList();
                    query = query.Where(x => ci3.Contains(x.ClientId));
                    break;
                case OBNLReinvestmentSearchBy.OBNLReinvestmentDate:
                    if (@params.TermFrom.HasValue)
                        query = query.Where(x => x.CreatedAt >= @params.TermFrom.Value);
                    if (@params.TermTo.HasValue)
                        query = query.Where(x => x.CreatedAt < @params.TermTo.Value.AddDays(1));
                    break;
            }
            
            return query;
        }
    }
}