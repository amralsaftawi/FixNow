using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Models;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;
using MediatR;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetTechnicianProfiles;

public sealed class GetTechnicianProfilesQueryHandler(
    ITechnicianProfileRepository technicianProfileRepository)
    : IQueryHandler<
        GetTechnicianProfilesQuery,
        Result<TechnicianProfilesResponse>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    public async Task<Result<TechnicianProfilesResponse>> Handle(
        GetTechnicianProfilesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _technicianProfileRepository.GetPagedAsync(
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            cancellationToken: cancellationToken);

        return new TechnicianProfilesResponse(
            Items: result.Items.ToDtos(),
            PageNumber: result.PageNumber,
            PageSize: result.PageSize,
            TotalCount: result.TotalCount,
            TotalPages: result.TotalPages);
    }

   
}