
public sealed class UserRole:AuditableEntity
{
    public Guid UserId { get;private set; } 

    public Guid RoleId { get;private set; } 

    public Guid? AssignedByUserId { get;private set; } 

     public bool IsActive { get; private set; }    

     public DateTimeOffset? RevokedAt { get; private set; }

    public Guid? RevokedByUserId { get; private set; }

    public User User { get; private set; } = null!;

    public Role Role { get; private set; } = null!;


#pragma warning disable CS8618
    private UserRole()
    {
    }
#pragma warning disable CS8618

    private UserRole(  Guid id,  Guid userId,  Guid roleId,Guid? assignedByUserId): base(id)
    {
        UserId = userId;

        RoleId = roleId;

        AssignedByUserId = assignedByUserId;

        IsActive = true;
    }

   public static Result<UserRole> Create( Guid id,  Guid userId,  Guid roleId,Guid? assignedByUserId)
    {

       if (id == Guid.Empty)
            return UserRoleErrors.IdRequired;

        if (userId == Guid.Empty)
            return UserRoleErrors.UserIdRequired;

        if (roleId == Guid.Empty)
            return UserRoleErrors.RoleIdRequired;
   
   var userRole = new UserRole(id, userId, roleId, assignedByUserId); 
  
    userRole.AddDomainEvent(
        new UserRoleAssignedDomainEvent(
            userRole.Id,
            userRole.UserId,
            userRole.RoleId));


        return userRole; 
    }

   public Result<Success> Revoke(Guid revokedByUserId)
    {
        if (revokedByUserId == Guid.Empty)
             return UserRoleErrors.RevokedByRequired;

        if (!IsActive)
            return UserRoleErrors.AlreadyRevoked;

        IsActive = false;

        RevokedAt = DateTimeOffset.UtcNow;

        RevokedByUserId = revokedByUserId;

        AddDomainEvent(
            new UserRoleRevokedDomainEvent(
                Id,
                UserId,
                RoleId));

        return Result.Success;
    } 


} 