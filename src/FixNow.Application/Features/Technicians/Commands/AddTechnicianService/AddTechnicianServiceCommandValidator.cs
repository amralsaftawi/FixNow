using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.AddTechnicianService;

public sealed class AddTechnicianServiceCommandValidator
    : AbstractValidator<AddTechnicianServiceCommand>
{
    public AddTechnicianServiceCommandValidator()
    {
        RuleFor(x => x.ServiceCategoryId)
            .NotEmpty()
            .WithErrorCode("TechnicianService.ServiceCategoryIdRequired");
    }
}
