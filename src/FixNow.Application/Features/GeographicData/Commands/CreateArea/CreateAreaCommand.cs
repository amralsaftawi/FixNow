using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.GeographicData.Commands.CreateArea;

public sealed record CreateAreaCommand(
    int CityId,
    string Name)
    : ICommand<Result<Created>>;
