using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.CreateTechnicianPortfolioItem;

public sealed record CreateTechnicianPortfolioItemCommand(
    string Title,
    string? Description,
    IReadOnlyCollection<string>? MediaKeys)
    : ICommand<Result<TechnicianPortfolioItemResponse>>;
