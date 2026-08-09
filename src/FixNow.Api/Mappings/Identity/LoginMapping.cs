using ApplicationLoginResponse = FixNow.Application.Features.Identity.Commands.Login.LoginResponse;
using ContractLoginResponse = FixNow.Contracts.Responses.LoginResponse;

namespace FixNow.Api.Mappings.Identity;

public static class LoginMapping
{
    public static ContractLoginResponse ToContractResponse(
        this ApplicationLoginResponse response)
        => new(
            AccessToken: response.AccessToken,
            AccessTokenExpiresAt: response.AccessTokenExpiresAt,
            RefreshToken: response.RefreshToken,
            RefreshTokenExpiresAt: response.RefreshTokenExpiresAt,
            TokenType: "Bearer");
}
