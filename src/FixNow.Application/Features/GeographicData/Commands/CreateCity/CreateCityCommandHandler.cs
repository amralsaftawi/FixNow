using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.GeographicData.Commands.CreateCity;

public sealed class CreateCityCommandHandler(
    ICityRepository cityRepository,
    ICountryRepository countryRepository)
    : ICommandHandler<CreateCityCommand, Result<Created>>
{
    public async Task<Result<Created>> Handle(
        CreateCityCommand command,
        CancellationToken cancellationToken)
    {
        if (!await countryRepository.ExistsByIdAsync(
                command.CountryId,
                cancellationToken))
        {
            return CountryErrors.NotFound;
        }

        var createResult = City.Create(
            command.CountryId,
            command.Name);

        if (createResult.IsError)
        {
            return createResult.Errors;
        }

        await cityRepository.AddAsync(
            createResult.Value,
            cancellationToken);

        return Result.Created;
    }
}
