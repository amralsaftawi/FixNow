
public sealed record UserRoleAssignedDomainEvent(
    Guid UserRoleId,
    Guid UserId,
    Guid RoleId) : DomainEvent;