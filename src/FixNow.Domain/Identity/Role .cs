public sealed class Role : AuditableEntity
{
    public string Name { get; private set; }

    public bool IsActive { get; private set; }

    private Role()
    {
    }

    private Role(Guid id, string name)
        : base(id)
    {
        Name = name;

        IsActive = true;
    }

    public static Result<Role> Create(
        Guid id,
        string name)
    {
        if (id == Guid.Empty)
            return RoleErrors.IdRequired;

        if (string.IsNullOrWhiteSpace(name))
            return RoleErrors.NameRequired;

        name = name.Trim();

        if (name.Length > 50)
            return RoleErrors.NameTooLong;

        return new Role(id, name);
    }

    public Result<Success> Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return RoleErrors.NameRequired;

        name = name.Trim();

        if (name.Length > 50)
            return RoleErrors.NameTooLong;

        Name = name;

        return Result.Success;
    }

    public Result<Success> Activate()
    {
        IsActive = true;

        return Result.Success;
    }

    public Result<Success> Deactivate()
    {
        IsActive = false;

        return Result.Success;
    }
}