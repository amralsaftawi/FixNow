using FluentValidation;

namespace FixNow.Application.Features.ServiceRequests.Commands.MarkSearching;

public sealed class MarkSearchingCommandValidator
    : AbstractValidator<MarkSearchingCommand>
{
    public MarkSearchingCommandValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");
    }
}
