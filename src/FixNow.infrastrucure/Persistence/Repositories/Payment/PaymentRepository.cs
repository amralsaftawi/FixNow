using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.Payment;

public sealed class PaymentRepository(AppDbContext dbContext) : IPaymentRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public Task AddAsync(
        global::Payment payment,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Payments.AddAsync(
            payment,
            cancellationToken).AsTask();
    }

    public Task<global::Payment?> GetByIdAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Payments
            .FirstOrDefaultAsync(
                payment => payment.Id == paymentId,
                cancellationToken);
    }

    public Task<bool> ExistsByAssignmentIdAsync(
        Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Payments
            .AsNoTracking()
            .AnyAsync(
                payment => payment.AssignmentId == assignmentId,
                cancellationToken);
    }

    public Task<global::Payment?> GetActiveByAssignmentIdAsync(
        Guid assignmentId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Payments
            .FirstOrDefaultAsync(
                payment => payment.AssignmentId == assignmentId
                    && payment.Status != global::PaymentStatus.Failed,
                cancellationToken);
    }
}
