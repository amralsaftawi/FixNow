

using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<OTPRecord> OTPRecords => Set<OTPRecord>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();

    public DbSet<CustomerProfile> CustomerProfiles => Set<CustomerProfile>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<CustomerPaymentMethod> CustomerPaymentMethods => Set<CustomerPaymentMethod>();

    public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();
    public DbSet<ProblemType> ProblemTypes => Set<ProblemType>();

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Area> Areas => Set<Area>();

    public DbSet<TechnicianProfile> TechnicianProfiles => Set<TechnicianProfile>();
    public DbSet<TechnicianService> TechnicianServices => Set<TechnicianService>();
    public DbSet<TechnicianExperience> TechnicianExperiences => Set<TechnicianExperience>();
    public DbSet<TechnicianPortfolioItem> TechnicianPortfolioItems => Set<TechnicianPortfolioItem>();
    public DbSet<TechnicianPortfolioMedia> TechnicianPortfolioMedia => Set<TechnicianPortfolioMedia>();

    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Review> Reviews => Set<Review>();

    public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();
    public DbSet<ServiceRequestImage> ServiceRequestImages => Set<ServiceRequestImage>();
    public DbSet<ServiceRequestTimeline> ServiceRequestTimelines => Set<ServiceRequestTimeline>();

    public DbSet<Assignment> Assignments => Set<Assignment>();

    public DbSet<Job> Jobs => Set<Job>();

    public DbSet<JobTimeline> JobTimelines => Set<JobTimeline>();

    public DbSet<JobAdditionalCharge> JobAdditionalCharges => Set<JobAdditionalCharge>();

    public DbSet<CustomerRating> CustomerRatings => Set<CustomerRating>();

    public DbSet<ReviewReport> ReviewReports => Set<ReviewReport>();

    public DbSet<TechnicianReport> TechnicianReports => Set<TechnicianReport>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

  
}

