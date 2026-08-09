namespace FixNow.Application.Features.ServiceCategories.Commands.UploadServiceCategoryIcon;

public static class UploadServiceCategoryIconMapping
{
    public static UploadServiceCategoryIconResponse ToUploadServiceCategoryIconResponse(
        this ServiceCategory entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new UploadServiceCategoryIconResponse(
            ServiceCategoryId: entity.Id,
            IconKey: entity.IconKey!);
    }
}
