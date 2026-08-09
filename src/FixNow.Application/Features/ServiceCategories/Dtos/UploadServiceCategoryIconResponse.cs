namespace FixNow.Application.Features.ServiceCategories.Commands.UploadServiceCategoryIcon;

public sealed record UploadServiceCategoryIconResponse(
    Guid ServiceCategoryId,
    string IconKey);
