using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Infrastructure.Authentication;
using FixNow.Infrastructure.Persistence.Repositories.ServiceCategory;
using FixNow.Infrastructure.Persistence.Repositories.Technician;
using FixNow.Infrastructure.Persistence.Repositories.User;
using FixNow.Infrastructure.Persistence.Repositories.Otp;
using FixNow.Infrastructure.Services;
using FixNow.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FixNow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString =configuration.GetConnectionString("DefaultConnection");

        ArgumentNullException.ThrowIfNull(connectionString);

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepository>();
        services.AddScoped<ITechnicianProfileRepository, TechnicianProfileRepository>();
        services.AddScoped<ITechnicianDiscoveryRepository, TechnicianDiscoveryRepository>();
        services.AddScoped<global::IUnitOfWork, global::FixNow.Infrastructure.UnitOfWork.UnitOfWork>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddSingleton<IPasswordHasher, PasswordHasherService>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IOtpRepository, OtpRepository>();

        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddSingleton<IRefreshTokenHasher, RefreshTokenHasher>();
        services.AddSingleton<IOtpGenerator, OtpGenerator>();
        services.AddSingleton<IOtpHasher, OtpHasher>();
        services.AddScoped<IEmailOtpSender, EmailOtpSender>();
        services.AddScoped<ISmsOtpSender, SmsOtpSender>();

        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
