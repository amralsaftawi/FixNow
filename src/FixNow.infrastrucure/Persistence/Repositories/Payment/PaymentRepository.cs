namespace FixNow.Infrastructure.Persistence.Repositories.Payment;

public sealed class PaymentRepository
{
    private readonly AppDbContext _dbContext;

    public PaymentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
