namespace FixNow.Contracts.Requests;

public sealed record UpdateTechnicianAccountStatusRequest
{
    public AccountStatus Status { get; init; }
}
