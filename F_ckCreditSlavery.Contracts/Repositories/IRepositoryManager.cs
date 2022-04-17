namespace F_ckCreditSlavery.Contracts.Repositories;

public interface IRepositoryManager
{
    ICreditAccountRepository CreditAccount { get; }
    ICreditAccountChangeRepository CreditAccountChangeRepository { get; }
    Task SaveAsync();
}