using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.RemoveTechnicianPortfolioItem;

public sealed record RemoveTechnicianPortfolioItemCommand(
    Guid PortfolioItemId)
    : ICommand<Result<Success>>;
