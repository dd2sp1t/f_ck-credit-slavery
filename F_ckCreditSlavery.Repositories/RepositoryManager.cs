using F_ckCreditSlavery.Entities;
using F_ckCreditSlavery.Contracts.Repositories;

namespace F_ckCreditSlavery.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
        
    private ICreditAccountRepository _creditAccountRepository = null!;
    private ICreditAccountChangeRepository _creditAccountChangeRepository = null!;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
    }

    public ICreditAccountRepository CreditAccount =>
        _creditAccountRepository ??= new CreditAccountRepository(_repositoryContext);

    public ICreditAccountChangeRepository CreditAccountChangeRepository =>
        _creditAccountChangeRepository ??= new CreditAccountChangeRepository(_repositoryContext);

    public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
}