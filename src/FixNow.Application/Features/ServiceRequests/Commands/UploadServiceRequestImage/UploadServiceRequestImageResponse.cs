namespace FixNow.Application.Features.ServiceRequests.Commands.UploadServiceRequestImage;

public sealed record UploadServiceRequestImageResponse(
    Guid ImageId,
    string ImageKey);
