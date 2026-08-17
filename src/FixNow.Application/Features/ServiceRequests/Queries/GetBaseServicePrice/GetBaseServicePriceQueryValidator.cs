using FluentValidation;

namespace FixNow.Application.Features.ServiceRequests.Queries.GetBaseServicePrice;

public sealed class GetBaseServicePriceQueryValidator
    : AbstractValidator<GetBaseServicePriceQuery>
{
    public GetBaseServicePriceQueryValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");
    }
}
