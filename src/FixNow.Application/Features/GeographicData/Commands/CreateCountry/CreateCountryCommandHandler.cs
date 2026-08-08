using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.GeographicData.Commands.CreateCountry;

public sealed class CreateCountryCommandHandler(
    ICountryRepository countryRepository)
    : ICommandHandler<CreateCountryCommand, Result<Created>>
{
    public async Task<Result<Created>> Handle(
        CreateCountryCommand command,
        CancellationToken cancellationToken)
    {
        if (await countryRepository.ExistsByNameAsync(
                command.Name,
                cancellationToken))
        {
            return CountryErrors.NameAlreadyExists;
        }

        var createResult = Country.Create(command.Name);

        if (createResult.IsError)
        {
            return createResult.Errors;
        }

        await countryRepository.AddAsync(
            createResult.Value,
            cancellationToken);

        return Result.Created;
    }
}
