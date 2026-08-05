using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace FixNow.Api.Endpoints.Identity;

public static class IdentityEndpointExtensions
{
    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapAuthEndpoints();
        endpoints.MapOtpEndpoints();
        endpoints.MapTokenEndpoints();
        endpoints.MapPasswordEndpoints();

        return endpoints;
    }
}
