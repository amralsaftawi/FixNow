namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface IOtpRepository
{
    Task<IReadOnlyCollection<OTPRecord>> GetActiveByUserAndPurposeAsync(
        Guid userId,
        OtpPurpose purpose,
        CancellationToken cancellationToken);

    Task<OTPRecord?> GetLatestAsync(
        Guid userId,
        OtpPurpose purpose,
        CancellationToken cancellationToken);

    Task AddAsync(
        OTPRecord otpRecord,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        OTPRecord otpRecord,
        CancellationToken cancellationToken);

    Task UpdateRangeAsync(
        IReadOnlyCollection<OTPRecord> otpRecords,
        CancellationToken cancellationToken);
}
