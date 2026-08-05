using Microsoft.EntityFrameworkCore.Storage;

namespace FixNow.Infrastructure.UnitOfWork;

public sealed class UnitOfWork (AppDbContext dbContext): global::IUnitOfWork
{
    private readonly AppDbContext _dbContext=dbContext;
    private IDbContextTransaction? _transaction;

    public async Task BeginTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            return;

        _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            return;

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _transaction.CommitAsync(cancellationToken);

        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            return;

        await _transaction.RollbackAsync(cancellationToken);

        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}