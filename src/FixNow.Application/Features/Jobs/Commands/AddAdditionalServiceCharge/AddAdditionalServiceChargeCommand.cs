using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Jobs.Commands.AddAdditionalServiceCharge;

public sealed record AddAdditionalServiceChargeCommand(
    Guid JobId,
    string Description,
    decimal Amount,
    Currency Currency)
    : ICommand<Result<AdditionalServiceChargeResponse>>;
