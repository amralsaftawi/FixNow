using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;


public sealed class RefreshTokenRepository(AppDbContext dbContext): IRefreshTokenRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task AddAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken)
    {
        await _dbContext.RefreshTokens.AddAsync(
            refreshToken,
            cancellationToken);
    }

    public async Task<RefreshToken?> GetByTokenHashAsync(
        string tokenHash,
        CancellationToken cancellationToken)
    {
        return await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(
                x => x.RefreshTokenHash == tokenHash,
                cancellationToken);
    }

    public async Task<RefreshToken?> GetWithUserByTokenHashAsync(
        string tokenHash,
        CancellationToken cancellationToken)
    {
        return await _dbContext.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.RefreshTokenHash == tokenHash,
                cancellationToken);
    }

    public async Task<bool> TryRevokeAsync(
        RefreshToken refreshToken,
        CancellationToken cancellationToken)
    {
        _dbContext.Entry(refreshToken).State = EntityState.Detached;

        var affectedRows = await _dbContext.RefreshTokens
            .Where(token =>
                token.Id == refreshToken.Id &&
                !token.IsRevoked &&
                token.ExpiresAt > DateTimeOffset.UtcNow)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(token => token.IsRevoked, true)
                    .SetProperty(token => token.RevokedAt, refreshToken.RevokedAt),
                cancellationToken);

        return affectedRows == 1;
    }

 
}
