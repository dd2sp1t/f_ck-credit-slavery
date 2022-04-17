using F_ckCreditSlavery.Entities;
using F_ckCreditSlavery.Entities.Models;
using F_ckCreditSlavery.Contracts.Repositories;
using F_ckCreditSlavery.Entities.RequestFeatures;
using F_ckCreditSlavery.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;

namespace F_ckCreditSlavery.Repositories;

public class CreditAccountRepository : RepositoryBase<CreditAccount>, ICreditAccountRepository
{
    public CreditAccountRepository(RepositoryContext repositoryContext)
        : base(repositoryContext) { }

    public async Task<PagedList<CreditAccount>> GetAsync(CreditAccountParameters parameters, bool trackChanges)
    {
        var accounts =  await FindAll(trackChanges)
            .Filter(parameters.MinStartDate, parameters.MaxStartDate, parameters.MinEndDate, parameters.MaxEndDate)
            .Search(parameters.SearchTerm)
            .Sort(parameters.OrderBy)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        var count = await FindAll(trackChanges)
            .Filter(parameters.MinStartDate, parameters.MaxStartDate, parameters.MinEndDate, parameters.MaxEndDate)
            .Search(parameters.SearchTerm)
            .CountAsync();
            
        return new PagedList<CreditAccount>(accounts, parameters.PageNumber, parameters.PageSize, count);
    }

    public Task<CreditAccount> GetAsync(int id, bool trackChanges)
    {
        return FindByCondition(e => e.Id == id, trackChanges)
            .SingleOrDefaultAsync();
    }

    public Task<List<CreditAccount>> GetAsync(IEnumerable<int> ids, bool trackChanges)
    {
        return FindByCondition(e => ids.Contains(e.Id), trackChanges)
            .ToListAsync();
    }

    public void CreateCreditAccount(CreditAccount creditAccount) => Create(creditAccount);
    public void DeleteCreditAccount(CreditAccount creditAccount) => Delete(creditAccount);
}