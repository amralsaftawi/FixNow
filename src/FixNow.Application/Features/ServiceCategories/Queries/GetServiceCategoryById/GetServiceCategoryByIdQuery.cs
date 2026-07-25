using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceCategories.Queries.GetServiceCategoryById;

public sealed record GetServiceCategoryByIdQuery(
    Guid ServiceCategoryId)
    : IQuery<Result<GetServiceCategoryByIdResponse>>;