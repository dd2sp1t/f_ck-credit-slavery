using System.Linq.Dynamic.Core;
using F_ckCreditSlavery.Entities.Models;
using F_ckCreditSlavery.Repositories.Extensions.Utility;

namespace F_ckCreditSlavery.Repositories.Extensions;

public static class CreditAccountRepositoryExtensions
{
    public static IQueryable<CreditAccount> Filter(
        this IQueryable<CreditAccount> creditAccounts,
        DateTime minStartDate,
        DateTime maxStartDate,
        DateTime minEndDate,
        DateTime maxEndDate) => creditAccounts
            .Where(e =>
                e.StartDate >= minStartDate &&
                e.StartDate <= maxStartDate &&
                e.EndDate >= minEndDate &&
                e.EndDate <= maxEndDate
            );

    public static IQueryable<CreditAccount> Search(
        this IQueryable<CreditAccount> creditAccounts,
        string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm)) return creditAccounts;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return creditAccounts.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
    }

    public static IQueryable<CreditAccount> Sort(
        this IQueryable<CreditAccount> creditAccounts,
        string orderByQueryString)
    {
        if (string.IsNullOrEmpty(orderByQueryString)) return creditAccounts.OrderByDescending(e => e.StartDate);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<CreditAccount>(orderByQueryString);
        
        return string.IsNullOrWhiteSpace(orderQuery) 
            ? creditAccounts.OrderByDescending(e => e.StartDate)
            : creditAccounts.OrderBy(orderQuery);
    }
}