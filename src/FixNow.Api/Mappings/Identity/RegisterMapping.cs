using ApplicationRegisterResponse = global::RegisterResponse;
using ContractRegisterResponse = FixNow.Contracts.Responses.RegisterResponse;

namespace FixNow.Api.Mappings.Identity;

public static class RegisterMapping
{
    public static ContractRegisterResponse ToContractResponse(
        this ApplicationRegisterResponse response)
        => new(
            UserId: response.UserId,
            Message: response.Message);
}
