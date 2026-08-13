namespace FixNow.Application.Common.Interfaces.Persistence.Repositories;

public interface IProblemTypeRepository
{
    Task<ProblemType?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
