using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.CustomerProfiles.Commands.CreateCustomerProfile;

public sealed record CreateCustomerProfileCommand
    : ICommand<Result<Created>>;
