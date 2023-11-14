using System;
using System.Linq;
using EcoCentre.Models.Domain.Clients;
using EcoCentre.Models.Domain.Common;
using EcoCentre.Models.Domain.User;
using EcoCentre.Models.Infrastructure;

namespace EcoCentre.Models.Domain.Invoices.Queries
{
    public class InvoiceListQuery
    {
        private readonly Repository<Invoice> _invoiceRepository;
        private readonly Repository<Client> _clientRepository;
        private readonly Repository<DeletedInvoice> _deletedInvoiceRepository;
        private readonly CenterIdentification _centerIdentification;
        private readonly AuthenticationContext _authContext;

        private const int PageSize = 20;
        public InvoiceListQuery(Repository<Invoice> invoiceRepository, Repository<Client> clientRepository, 
            Repository<DeletedInvoice> deletedInvoiceRepository, CenterIdentification centerIdentification,
            AuthenticationContext authContext)
        {
            _invoiceRepository = invoiceRepository;
            _clientRepository = clientRepository;
            _deletedInvoiceRepository = deletedInvoiceRepository;

            _centerIdentification = centerIdentification;
            _authContext = authContext;
        }

        public InvoiceListQueryResult Execute(InvoiceListQueryParams @params)
        {
            if (@params.Deleted)
                return DeletedInvoiceList(@params);
            return InvoiceList(@params);
        }

        private InvoiceListQueryResult DeletedInvoiceList(InvoiceListQueryParams @params)
        {
            if (@params.Page < 1) @params.Page = 1;
            var skip = (@params.Page - 1) * (@params.PageSize ?? PageSize);
            var pageSize = @params.PageSize ?? PageSize;
            var query = _deletedInvoiceRepository.Query;

            if (!_authContext.User.IsGlobalAdmin)
            {
                query = query.Where(x => x.Invoice.Center != null && x.Invoice.Center.Name == _centerIdentification.Name);
            }

            var total = query.Count();
            query = query.OrderByDescending(x => x.DeletedAt);

            if (pageSize > 0)
            {
                query = query.Skip(skip).Take(pageSize);
            }
            var invoices = query.ToList();
            var pageCount = pageSize > 0 ? (int)Math.Ceiling(total / (double)pageSize) : 1;

            var clientIds = invoices.Select(x => x.Invoice.ClientId);
            var clients = _clientRepository.Query.Where(x => clientIds.Contains(x.Id)).ToList();
            var invoiceDetails = invoices.Select(i => new InvoiceDetails(i.Invoice, clients.FirstOrDefault(x => x.Id == i.Invoice.ClientId)));

            return new InvoiceListQueryResult
            {
                Invoices = invoiceDetails,
                Total = total,
                Page = @params.Page,
                PageCount = pageCount
            };
        }

        private InvoiceListQueryResult InvoiceList(InvoiceListQueryParams @params)
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
            var clients = _clientRepository.Query.Where(x => clientIds.Contains(x.Id)).ToList();
            var invoiceDetails = invoices.Select(i => new InvoiceDetails(i, clients.FirstOrDefault(x => x.Id == i.ClientId)));

            return new InvoiceListQueryResult
                {
                    Invoices = invoiceDetails,
                    Total = total,
                    Page = @params.Page,
                    PageCount = pageCount
                };

        }

        private static IQueryable<Invoice> ApplySort(InvoiceListQueryParams @params, IQueryable<Invoice> query)
        {
            switch (@params.SortBy)
            {
                case InvoiceSortTerm.InvoiceDate:
                    query = query.OrderBy(x => x.CreatedAt, @params.SortDir);
                    break;
                case InvoiceSortTerm.InvoiceNo:
                    query = query.OrderBy(x => x.SequentialNo, @params.SortDir);
                    break;
            }
            return query;
        }

        private IQueryable<Invoice> ApplySearchTerm(InvoiceListQueryParams @params, IQueryable<Invoice> query)
        {
            if (!string.IsNullOrEmpty(@params.Term)) @params.Term = @params.Term.Trim();
            if (_authContext.User.IsGlobalAdmin && !string.IsNullOrEmpty(@params.CenterName) && @params.CenterName.ToLower() != "tous")
            {
                query = query.Where(x => x.Center != null && x.Center.Name.Equals(@params.CenterName));
            }

            switch (@params.Type)
            {
                case InvoiceSearchBy.Number:
	                if (string.IsNullOrEmpty(@params.Term))
	                {
		                break;
	                }
                    query = query.Where(x => x.InvoiceNo.ToLower().Contains(@params.Term.ToLower()));
                    break;
                case InvoiceSearchBy.ClientFirstName:
                    if (string.IsNullOrEmpty(@params.Term)) break;
                    var ci1 =
                        _clientRepository.Query.Where(x => x.FirstNameLower.Contains(@params.Term.ToLower()))
                                            .Select(x => x.Id)
                                            .ToList();
                    query = query.Where(x => ci1.Contains(x.ClientId));
                    break;
                case InvoiceSearchBy.ClientLastName:
                    if (string.IsNullOrEmpty(@params.Term)) break;
                    var ci2 =
                        _clientRepository.Query.Where(x => x.LastNameLower.Contains(@params.Term.ToLower()))
                                            .Select(x => x.Id)
                                            .ToList();
                    query = query.Where(x => ci2.Contains(x.ClientId));
                    break;
                case InvoiceSearchBy.ClientAddress:
                    if (string.IsNullOrEmpty(@params.Term)) break;
                    var ci3 =
                        _clientRepository.Query.Where(x => x.Address.City.Contains(@params.Term) || x.Address.StreetLower.Contains(@params.Term.ToLower()))
                                            .Select(x => x.Id)
                                            .ToList();
                    query = query.Where(x => ci3.Contains(x.ClientId));
                    break;
                case InvoiceSearchBy.InvoiceDate:
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