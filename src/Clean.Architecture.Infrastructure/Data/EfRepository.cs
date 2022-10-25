using Ardalis.Specification.EntityFrameworkCore;
using Clean.Architecture.SharedKernel.Interfaces;

namespace Clean.Architecture.Infrastructure.Data;

// inherit from Ardalis.Specification type
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>, IConfigureUnitOfWork where T : class, IAggregateRoot
{
  public bool UseUnitOfWork { get; set; } = false;
  private readonly AppDbContext _dbContext;

  public EfRepository(AppDbContext dbContext) : base(dbContext)
  {
    _dbContext = dbContext;
  }

  public override async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
  {
    _dbContext.Set<T>().Remove(entity);
    if (!UseUnitOfWork)
    {
      await _dbContext.SaveChangesAsync(cancellationToken);
    }
  }

  public override async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
  {
    _dbContext.Set<T>().Update(entity);
    if (!UseUnitOfWork)
    {
      await _dbContext.SaveChangesAsync(cancellationToken);
    }
  }
}

public interface IConfigureUnitOfWork
{
  bool UseUnitOfWork { get; set; }
}
