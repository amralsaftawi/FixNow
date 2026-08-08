namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface ICustomerRepository
{
    Task<CustomerProfile?> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        CustomerProfile customerProfile,
        CancellationToken cancellationToken = default);

    void Update(CustomerProfile customerProfile);
}
