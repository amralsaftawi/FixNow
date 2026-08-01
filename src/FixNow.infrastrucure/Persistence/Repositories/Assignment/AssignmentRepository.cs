namespace FixNow.Infrastructure.Persistence.Repositories.Assignment;

public sealed class AssignmentRepository
{
    private readonly AppDbContext _dbContext;

    public AssignmentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
