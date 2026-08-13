using FluentValidation;

namespace FixNow.Application.Features.ServiceRequests.Commands.CreateServiceRequest;

public sealed class CreateServiceRequestCommandValidator
    : AbstractValidator<CreateServiceRequestCommand>
{
    public CreateServiceRequestCommandValidator()
    {
        ValidateAddressId();

        ValidateServiceCategoryId();

        ValidateDescription();

        ValidatePriority();

        ValidateScheduledAt();
    }

    private void ValidateAddressId()
    {
        RuleFor(x => x.AddressId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.AddressIdRequired");
    }

    private void ValidateServiceCategoryId()
    {
        RuleFor(x => x.ServiceCategoryId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.ServiceCategoryIdRequired");
    }

    private void ValidateDescription()
    {
        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.DescriptionRequired")
            .MaximumLength(2000)
            .WithErrorCode("ServiceRequest.DescriptionTooLong");
    }

    private void ValidatePriority()
    {
        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithErrorCode("ServiceRequest.Priority.Invalid");
    }

    private void ValidateScheduledAt()
    {
        RuleFor(x => x.ScheduledAt)
            .Must(value => !value.HasValue || value.Value > DateTimeOffset.UtcNow)
            .WithErrorCode("ServiceRequest.InvalidScheduleDate");
    }
}
