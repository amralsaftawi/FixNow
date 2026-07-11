
public sealed class ServiceCategory : AuditableEntity
{
    public string Name { get; private set; }

    public string Description { get; private set; }

    public string? IconKey { get; private set; }

    public int DisplayOrder { get; private set; }

    public bool IsActive { get; private set; }

#pragma warning disable CS8618
    private ServiceCategory()
    {
    }
#pragma warning disable CS8618
    private ServiceCategory(
        Guid id,
        string name,
        string description,
        string iconKey,
        int displayOrder)
        : base(id)
    {
        Name = name;
        Description = description;
        IconKey = iconKey;
        DisplayOrder = displayOrder;

        IsActive = true;
    }

    public static Result<ServiceCategory> Create(
        Guid id,
        string name,
        string description,
        string iconKey,
        int displayOrder)
    {
        if (id == Guid.Empty)
            return ServiceCategoryErrors.IdRequired;

        if (string.IsNullOrWhiteSpace(name))
            return ServiceCategoryErrors.NameRequired;

        name = name.Trim();

        if (name.Length > 100)
            return ServiceCategoryErrors.NameTooLong;

        description = description?.Trim() ?? string.Empty;

        if (description.Length > 500)
            return ServiceCategoryErrors.DescriptionTooLong;

        if (string.IsNullOrWhiteSpace(iconKey))
            return ServiceCategoryErrors.IconKeyRequired;

        if (displayOrder < 0)
            return ServiceCategoryErrors.InvalidDisplayOrder;

        var category = new ServiceCategory(
            id,
            name,
            description,
            iconKey.Trim(),
            displayOrder);

        category.AddDomainEvent(
            new ServiceCategoryCreatedDomainEvent(category.Id));

        return category;
    }

    public Result<Success> Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ServiceCategoryErrors.NameRequired;

        name = name.Trim();

        if (name.Length > 100)
            return ServiceCategoryErrors.NameTooLong;

        if (Name == name)
            return ServiceCategoryErrors.SameName;

        Name = name;

        AddDomainEvent(
            new ServiceCategoryRenamedDomainEvent(Id));

        return Result.Success;
    }

    public Result<Success> ChangeDescription(string description)
    {
        description = description?.Trim() ?? string.Empty;

        if (description.Length > 500)
            return ServiceCategoryErrors.DescriptionTooLong;

        if (Description == description)
            return ServiceCategoryErrors.SameDescription;

        Description = description;

        return Result.Success;
    }

    public Result<Success> ChangeIcon(string iconKey)
    {
        if (string.IsNullOrWhiteSpace(iconKey))
            return ServiceCategoryErrors.IconKeyRequired;

        iconKey = iconKey.Trim();

        if (IconKey == iconKey)
            return ServiceCategoryErrors.SameIcon;

        IconKey = iconKey;

        return Result.Success;
    }

    public Result<Success> ChangeDisplayOrder(int displayOrder)
    {
        if (displayOrder < 0)
            return ServiceCategoryErrors.InvalidDisplayOrder;

        if (DisplayOrder == displayOrder)
            return ServiceCategoryErrors.SameDisplayOrder;

        DisplayOrder = displayOrder;

        return Result.Success;
    }

    public Result<Success> Activate()
    {
        if (IsActive)
            return ServiceCategoryErrors.AlreadyActive;

        IsActive = true;

        AddDomainEvent(
            new ServiceCategoryActivatedDomainEvent(Id));

        return Result.Success;
    }

    public Result<Success> Deactivate()
    {
        if (!IsActive)
            return ServiceCategoryErrors.AlreadyInactive;

        IsActive = false;

        AddDomainEvent(
            new ServiceCategoryDeactivatedDomainEvent(Id));

        return Result.Success;
    }
}