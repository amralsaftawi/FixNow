using ApplicationRefreshTokenResponse = FixNow.Application.Features.Identity.Commands.RefreshToken.RefreshTokenResponse;
using ContractRefreshTokenResponse = FixNow.Contracts.Responses.RefreshTokenResponse;

namespace FixNow.Api.Mappings.Identity;

public static class RefreshTokenMapping
{
    public static ContractRefreshTokenResponse ToContractResponse(
        this ApplicationRefreshTokenResponse response)
        => new(
            AccessToken: response.AccessToken,
            AccessTokenExpiresAt: response.AccessTokenExpiresAt,
            RefreshToken: response.RefreshToken,
            RefreshTokenExpiresAt: response.RefreshTokenExpiresAt);
}
