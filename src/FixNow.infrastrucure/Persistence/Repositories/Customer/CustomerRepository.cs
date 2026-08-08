using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.Customer;

public sealed class CustomerRepository(AppDbContext dbContext) : ICustomerRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<CustomerProfile?> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.CustomerProfiles
            .Include(profile => profile.Addresses)
            .FirstOrDefaultAsync(
                profile => profile.UserId == userId,
                cancellationToken);
    }

    public Task<bool> ExistsByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.CustomerProfiles
            .AsNoTracking()
            .AnyAsync(
                profile => profile.UserId == userId,
                cancellationToken);
    }

    public async Task AddAsync(
        CustomerProfile customerProfile,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.CustomerProfiles.AddAsync(
            customerProfile,
            cancellationToken);
    }

    public void Update(CustomerProfile customerProfile)
    {
        _dbContext.CustomerProfiles.Update(customerProfile);
    }
}
