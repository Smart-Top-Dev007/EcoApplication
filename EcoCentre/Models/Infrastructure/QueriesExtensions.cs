using System;
using System.Linq;
using System.Linq.Expressions;
using EcoCentre.Models.Domain.Common;
using EcoCentre.Models.Domain.Invoices;

namespace EcoCentre.Models.Infrastructure
{
    public static class QueriesExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T,TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> sort, SortDir sortDir)
        {
            if (sortDir == SortDir.Asc)
                return query.OrderBy(sort);
            return query.OrderByDescending(sort);
        }
    }
}