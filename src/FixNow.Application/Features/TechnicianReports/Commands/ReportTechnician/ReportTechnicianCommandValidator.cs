using FluentValidation;

namespace FixNow.Application.Features.TechnicianReports.Commands.ReportTechnician;

public sealed class ReportTechnicianCommandValidator
    : AbstractValidator<ReportTechnicianCommand>
{
    public ReportTechnicianCommandValidator()
    {
        RuleFor(x => x.TechnicianProfileId)
            .NotEmpty()
            .WithErrorCode("TechnicianReport.TechnicianProfileIdRequired");

        RuleFor(x => x.Reason)
            .IsInEnum()
            .WithErrorCode("TechnicianReport.ReasonInvalid");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithErrorCode("TechnicianReport.DescriptionTooLong");
    }
}
