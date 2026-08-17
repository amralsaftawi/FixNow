namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface IPaymentRepository
{
    Task AddAsync(
        Payment payment,
        CancellationToken cancellationToken = default);

    Task<Payment?> GetByIdAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByAssignmentIdAsync(
        Guid assignmentId,
        CancellationToken cancellationToken = default);

    Task<Payment?> GetActiveByAssignmentIdAsync(
        Guid assignmentId,
        CancellationToken cancellationToken = default);
}
