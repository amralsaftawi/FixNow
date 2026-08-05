using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.User;

public sealed class UserRepository (AppDbContext dbContext): IUserRepository
{
    private readonly AppDbContext _dbContext=dbContext;

    public async Task AddAsync(global::User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.Email != null && user.Email.Value == email.Value, cancellationToken);
    }

    public Task<bool> ExistsByPhoneNumberAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.PhoneNumber.Value == phoneNumber.Value, cancellationToken);
    }

    public Task<global::User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email != null && user.Email.Value == email, cancellationToken);
    }

    public Task<global::User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    public Task<global::User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.PhoneNumber.Value == phoneNumber, cancellationToken);
    }

    public void Remove(global::User user)
    {
        _dbContext.Users.Remove(user);
    }

    public void Update(global::User user)
    {
        _dbContext.Users.Update(user);
    }
}