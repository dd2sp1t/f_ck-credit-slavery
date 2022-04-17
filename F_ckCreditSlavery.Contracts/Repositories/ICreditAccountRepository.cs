using F_ckCreditSlavery.Entities.Models;
using F_ckCreditSlavery.Entities.RequestFeatures;

namespace F_ckCreditSlavery.Contracts.Repositories;

public interface ICreditAccountRepository
{
    Task<PagedList<CreditAccount>> GetAsync(CreditAccountParameters parameters, bool trackChanges);
    Task<List<CreditAccount>> GetAsync(IEnumerable<int> ids, bool trackChanges);
    Task<CreditAccount> GetAsync(int id, bool trackChanges);
    void CreateCreditAccount(CreditAccount creditAccount);
    void DeleteCreditAccount(CreditAccount creditAccount);
}