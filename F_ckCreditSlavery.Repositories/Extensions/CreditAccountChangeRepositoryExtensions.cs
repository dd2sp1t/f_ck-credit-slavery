using System.Linq.Dynamic.Core;
using F_ckCreditSlavery.Entities.Models;
using F_ckCreditSlavery.Entities.RequestFeatures;
using F_ckCreditSlavery.Repositories.Extensions.Utility;

namespace F_ckCreditSlavery.Repositories.Extensions;

public static class CreditAccountChangeRepositoryExtensions
{
    public static IQueryable<CreditAccountChange> Filter(
        this IQueryable<CreditAccountChange> changes,
        CreditAccountChangeParameters parameters)
    {
        return changes.Where(e =>
            e.PaymentDate >= parameters.MinDate &&
            e.PaymentDate <= parameters.MaxDate);
    }

    public static IQueryable<CreditAccountChange> Sort(
        this IQueryable<CreditAccountChange> changes,
        string orderByQueryString)
    {
        if (string.IsNullOrEmpty(orderByQueryString)) return changes.OrderByDescending(e => e.PaymentDate);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<CreditAccountChange>(orderByQueryString);

        return string.IsNullOrWhiteSpace(orderQuery) 
            ? changes.OrderByDescending(e => e.PaymentDate) 
            : changes.OrderBy(orderQuery);
    }
}