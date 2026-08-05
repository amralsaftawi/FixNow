using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.Otp;

public sealed class OtpRepository(AppDbContext dbContext) : IOtpRepository
{
    public async Task<IReadOnlyCollection<OTPRecord>> GetActiveByUserAndPurposeAsync(
        Guid userId,
        OtpPurpose purpose,
        CancellationToken cancellationToken)
    {
        return await dbContext.OTPRecords
            .Where(otpRecord =>
                otpRecord.UserId == userId &&
                otpRecord.Purpose == purpose &&
                otpRecord.VerifiedAt == null &&
                otpRecord.InvalidatedAt == null &&
                otpRecord.ExpiresAt > DateTimeOffset.UtcNow)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        OTPRecord otpRecord,
        CancellationToken cancellationToken)
    {
        await dbContext.OTPRecords.AddAsync(otpRecord, cancellationToken);
    }

    public Task UpdateRangeAsync(
        IReadOnlyCollection<OTPRecord> otpRecords,
        CancellationToken cancellationToken)
    {
        dbContext.OTPRecords.UpdateRange(otpRecords);

        return Task.CompletedTask;
    }
}
