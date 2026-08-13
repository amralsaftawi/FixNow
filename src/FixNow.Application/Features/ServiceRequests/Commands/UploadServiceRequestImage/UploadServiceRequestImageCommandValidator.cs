using FluentValidation;

namespace FixNow.Application.Features.ServiceRequests.Commands.UploadServiceRequestImage;

public sealed class UploadServiceRequestImageCommandValidator
    : AbstractValidator<UploadServiceRequestImageCommand>
{
    public UploadServiceRequestImageCommandValidator()
    {
        ValidateFile();
    }

    private void ValidateFile()
    {
        RuleFor(x => x.Content)
            .NotNull()
            .WithErrorCode("ServiceRequest.Image.File.Required");

        RuleFor(x => x.FileName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.Image.FileName.Required")
            .Must(HaveAnAllowedExtension)
            .WithErrorCode("ServiceRequest.Image.File.TypeNotAllowed");

        RuleFor(x => x.ContentType)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.Image.ContentType.Required")
            .Must(HaveAnAllowedContentType)
            .WithErrorCode("ServiceRequest.Image.File.TypeNotAllowed");

        RuleFor(x => x.ContentLength)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithErrorCode("ServiceRequest.Image.File.Empty")
            .LessThanOrEqualTo(UploadServiceRequestImageCommand.MaxFileSizeBytes)
            .WithErrorCode("ServiceRequest.Image.File.TooLarge");
    }

    private static bool HaveAnAllowedExtension(string fileName)
    {
        return UploadServiceRequestImageCommand.AllowedExtensions.Contains(
            Path.GetExtension(fileName));
    }

    private static bool HaveAnAllowedContentType(string contentType)
    {
        return UploadServiceRequestImageCommand.AllowedContentTypes.Contains(
            contentType.Trim());
    }
}
