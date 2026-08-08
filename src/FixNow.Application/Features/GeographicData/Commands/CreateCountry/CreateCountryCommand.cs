using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.GeographicData.Commands.CreateCountry;

public sealed record CreateCountryCommand(
    string Name)
    : ICommand<Result<Created>>;
