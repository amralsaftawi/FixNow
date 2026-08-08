using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.GeographicData.Commands.CreateArea;

public sealed class CreateAreaCommandHandler(
    IAreaRepository areaRepository,
    ICityRepository cityRepository)
    : ICommandHandler<CreateAreaCommand, Result<Created>>
{
    public async Task<Result<Created>> Handle(
        CreateAreaCommand command,
        CancellationToken cancellationToken)
    {
        if (!await cityRepository.ExistsByIdAsync(
                command.CityId,
                cancellationToken))
        {
            return CityErrors.NotFound;
        }

        var createResult = Area.Create(
            command.CityId,
            command.Name);

        if (createResult.IsError)
        {
            return createResult.Errors;
        }

        await areaRepository.AddAsync(
            createResult.Value,
            cancellationToken);

        return Result.Created;
    }
}
