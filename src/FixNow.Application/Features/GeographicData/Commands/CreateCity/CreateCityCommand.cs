using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.GeographicData.Commands.CreateCity;

public sealed record CreateCityCommand(
    int CountryId,
    string Name)
    : ICommand<Result<Created>>;
