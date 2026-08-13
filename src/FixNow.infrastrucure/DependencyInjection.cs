using System.Text;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Infrastructure.Authentication;
using FixNow.Infrastructure.Options;
using FixNow.Infrastructure.Persistence.Repositories.ServiceCategory;
using FixNow.Infrastructure.Persistence.Repositories.Technician;
using FixNow.Infrastructure.Persistence.Repositories.User;
using FixNow.Infrastructure.Persistence.Repositories.Otp;
using FixNow.Infrastructure.Persistence.Repositories.Customer;
using FixNow.Infrastructure.Persistence.Repositories.Geographic;
using FixNow.Infrastructure.Persistence.Repositories.ServiceRequests;
using FixNow.Infrastructure.Persistence.Repositories.Assignments;
using FixNow.Infrastructure.Persistence.Repositories.ProblemTypes;
using FixNow.Infrastructure.Services;
using FixNow.Infrastructure.UnitOfWork;
using FixNow.Application.Features.Identity.Commands.VerifyOtp.Processors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FixNow.Application.Common.Interfaces.Authentication;
using FixNow.Application.Common.Interfaces.Storage;
using FixNow.Infrastructure.Storage;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FixNow.Infrastructure;

public static class DependencyInjection
{
    private static readonly TimeSpan HttpRequestTimeout =
        TimeSpan.FromSeconds(15);

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
        services.AddScoped<ITechnicianSearchRepository, TechnicianSearchRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IAreaRepository, AreaRepository>();
        services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IProblemTypeRepository, ProblemTypeRepository>();
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
        services.AddScoped<IOtpSender, EmailOtpSender>();

        services.AddScoped<ITokenService, TokenService>();

        var jwtSection = configuration.GetSection("Jwt");

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                jwtSection["SecretKey"]
                ?? throw new InvalidOperationException(
                    "JWT SecretKey is not configured.")));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = signingKey,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });

        services.AddAuthorization();

        services.AddScoped<IFileStorage, CloudinaryFileStorage>();

        services.Configure<CloudinaryOptions>(
            configuration.GetSection(CloudinaryOptions.SectionName));

        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<CloudinaryOptions>>().Value;

            return new CloudinaryDotNet.Cloudinary(
                new CloudinaryDotNet.Account(
                    options.CloudName,
                    options.ApiKey,
                    options.ApiSecret));
        });

        services.AddScoped<IOtpPurposeProcessor, EmailVerificationProcessor>();
        services.AddScoped<IOtpPurposeProcessor, PhoneVerificationProcessor>();


        return services;
    }
}
