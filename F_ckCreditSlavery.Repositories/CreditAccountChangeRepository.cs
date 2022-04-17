using F_ckCreditSlavery.Entities;
using F_ckCreditSlavery.Entities.Models;
using F_ckCreditSlavery.Contracts.Repositories;
using F_ckCreditSlavery.Entities.RequestFeatures;
using F_ckCreditSlavery.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace F_ckCreditSlavery.Repositories;

public class CreditAccountChangeRepository : RepositoryBase<CreditAccountChange>, ICreditAccountChangeRepository
{
    public CreditAccountChangeRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

    public async Task<PagedList<CreditAccountChange>> GetAsync(
        int creditAccountId,
        bool trackChanges,
        CreditAccountChangeParameters parameters)
    {
        var changes = await FindByCondition(
                e => e.CreditAccountId.Equals(creditAccountId),
                trackChanges)
            .Filter(parameters)
            .Sort(parameters.OrderBy)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        var count = await FindByCondition(
                e => e.CreditAccountId.Equals(creditAccountId), 
                trackChanges)
            .Filter(parameters)
            .CountAsync();
            
        return new PagedList<CreditAccountChange>(changes, parameters.PageNumber, parameters.PageSize, count);
    }

    public Task<CreditAccountChange> GetAsync(int id, bool trackChanges)
    {
        return FindByCondition(e => e.Id == id, trackChanges)
            .SingleOrDefaultAsync();
    }

    public void CreateCreditAccountChange(int creditAccountId, CreditAccountChange creditAccountChange)
    {
        creditAccountChange.CreditAccountId = creditAccountId;
        Create(creditAccountChange);
    }

    public void DeleteCreditAccountChange(CreditAccountChange creditAccountChange) => Delete(creditAccountChange);
}