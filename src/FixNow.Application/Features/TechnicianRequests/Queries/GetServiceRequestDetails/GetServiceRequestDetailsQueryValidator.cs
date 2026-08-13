using FluentValidation;

namespace FixNow.Application.Features.TechnicianRequests.Queries.GetServiceRequestDetails;

public sealed class GetServiceRequestDetailsQueryValidator
    : AbstractValidator<GetServiceRequestDetailsQuery>
{
    public GetServiceRequestDetailsQueryValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");
    }
}
