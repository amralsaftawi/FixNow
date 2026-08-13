using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FixNow.Infrastructure.Persistence.Repositories.ProblemTypes;

public sealed class ProblemTypeRepository(AppDbContext dbContext)
    : IProblemTypeRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public Task<ProblemType?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.ProblemTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(
                problemType => problemType.Id == id,
                cancellationToken);
    }
}
