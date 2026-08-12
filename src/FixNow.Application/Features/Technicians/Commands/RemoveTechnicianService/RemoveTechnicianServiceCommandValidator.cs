using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.RemoveTechnicianService;

public sealed class RemoveTechnicianServiceCommandValidator
    : AbstractValidator<RemoveTechnicianServiceCommand>
{
    public RemoveTechnicianServiceCommandValidator()
    {
        RuleFor(x => x.ServiceCategoryId)
            .NotEmpty()
            .WithErrorCode("TechnicianService.CategoryIdRequired");
    }
}
