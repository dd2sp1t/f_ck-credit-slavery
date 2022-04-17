using F_ckCreditSlavery.Entities.Models;
using F_ckCreditSlavery.Entities.RequestFeatures;

namespace F_ckCreditSlavery.Contracts.Repositories;

public interface ICreditAccountChangeRepository
{
    Task<PagedList<CreditAccountChange>> GetAsync(int creditAccountId, bool trackChanges, CreditAccountChangeParameters parameters);
    Task<CreditAccountChange> GetAsync(int id, bool trackChanges);
    void CreateCreditAccountChange(int creditAccountId, CreditAccountChange creditAccountChange);
    void DeleteCreditAccountChange(CreditAccountChange creditAccountChange);
}