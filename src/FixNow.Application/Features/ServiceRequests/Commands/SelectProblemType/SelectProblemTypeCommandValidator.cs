using FluentValidation;

namespace FixNow.Application.Features.ServiceRequests.Commands.SelectProblemType;

public sealed class SelectProblemTypeCommandValidator
    : AbstractValidator<SelectProblemTypeCommand>
{
    public SelectProblemTypeCommandValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");

        RuleFor(x => x.ProblemTypeId)
            .NotEmpty()
            .WithErrorCode("ProblemType.IdRequired");
    }
}
