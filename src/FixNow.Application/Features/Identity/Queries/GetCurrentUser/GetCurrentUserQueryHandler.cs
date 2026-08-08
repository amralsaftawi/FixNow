using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.Identity.Queries.GetCurrentUser;

public sealed class GetCurrentUserQueryHandler(
    ICurrentUser currentUser,
    IUserRepository userRepository)
    : IQueryHandler<GetCurrentUserQuery, Result<GetCurrentUserResponse>>
{
    public async Task<Result<GetCurrentUserResponse>> Handle(
        GetCurrentUserQuery query,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated)
            return IdentityErrors.Unauthorized;

        var user = await userRepository.GetByIdAsync(currentUser.UserId,cancellationToken);

        if (user is null)
            return UserErrors.NotFound;

        return new GetCurrentUserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email?.Value,
            user.PhoneNumber.Value);
    }
}