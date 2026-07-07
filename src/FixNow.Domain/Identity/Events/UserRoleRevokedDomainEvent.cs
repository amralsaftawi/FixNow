
public sealed record UserRoleRevokedDomainEvent(
    Guid UserRoleId,
    Guid UserId,
    Guid RoleId) : DomainEvent;