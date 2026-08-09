using FluentValidation;

namespace FixNow.Application.Features.ServiceCategories.Commands.UploadServiceCategoryIcon;

public sealed class UploadServiceCategoryIconCommandValidator
    : AbstractValidator<UploadServiceCategoryIconCommand>
{
    public UploadServiceCategoryIconCommandValidator()
    {
        ValidateServiceCategoryId();
        ValidateFile();
    }

    private void ValidateServiceCategoryId()
    {
        RuleFor(x => x.ServiceCategoryId)
            .NotEmpty().WithErrorCode("ServiceCategory.Id.Required");
    }

    private void ValidateFile()
    {
        RuleFor(x => x.Content)
            .NotNull().WithErrorCode("ServiceCategory.Icon.File.Required");

        RuleFor(x => x.FileName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("ServiceCategory.Icon.FileName.Required")
            .Must(HaveAnAllowedExtension)
                .WithErrorCode("ServiceCategory.Icon.File.TypeNotAllowed");

        RuleFor(x => x.ContentLength)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithErrorCode("ServiceCategory.Icon.File.Empty")
            .LessThanOrEqualTo(UploadServiceCategoryIconCommand.MaxFileSizeBytes)
                .WithErrorCode("ServiceCategory.Icon.File.TooLarge");
    }

    private static bool HaveAnAllowedExtension(string fileName)
    {
        return UploadServiceCategoryIconCommand.AllowedExtensions.Contains(
            Path.GetExtension(fileName));
    }
}
