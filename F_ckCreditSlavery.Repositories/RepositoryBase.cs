using System.Linq.Expressions;
using F_ckCreditSlavery.Entities;
using F_ckCreditSlavery.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace F_ckCreditSlavery.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly RepositoryContext RepositoryContext;

    protected RepositoryBase(RepositoryContext repositoryContext)
    {
        RepositoryContext = repositoryContext;
    }

    public IQueryable<T> FindAll(bool trackChanges) => 
        trackChanges ? 
            RepositoryContext.Set<T>() :
            RepositoryContext.Set<T>().AsNoTracking();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
        trackChanges ? 
            RepositoryContext.Set<T>().Where(expression) :
            RepositoryContext.Set<T>().Where(expression).AsNoTracking();

    public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);

    public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);

    public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
}