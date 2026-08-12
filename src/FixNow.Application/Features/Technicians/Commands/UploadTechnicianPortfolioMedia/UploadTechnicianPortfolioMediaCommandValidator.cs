using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UploadTechnicianPortfolioMedia;

public sealed class UploadTechnicianPortfolioMediaCommandValidator
    : AbstractValidator<UploadTechnicianPortfolioMediaCommand>
{
    public UploadTechnicianPortfolioMediaCommandValidator()
    {
        ValidateFile();
    }

    private void ValidateFile()
    {
        RuleFor(x => x.Content)
            .NotNull()
            .WithErrorCode("TechnicianPortfolio.Media.File.Required");

        RuleFor(x => x.FileName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("TechnicianPortfolio.Media.FileName.Required")
            .Must(HaveAnAllowedExtension)
            .WithErrorCode("TechnicianPortfolio.Media.File.TypeNotAllowed");

        RuleFor(x => x.ContentLength)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithErrorCode("TechnicianPortfolio.Media.File.Empty")
            .LessThanOrEqualTo(UploadTechnicianPortfolioMediaCommand.MaxFileSizeBytes)
            .WithErrorCode("TechnicianPortfolio.Media.File.TooLarge");
    }

    private static bool HaveAnAllowedExtension(string fileName)
    {
        return UploadTechnicianPortfolioMediaCommand.AllowedExtensions.Contains(
            Path.GetExtension(fileName));
    }
}
