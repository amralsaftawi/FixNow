using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Assignments.Commands.AssignServiceRequest;

public sealed class AssignServiceRequestCommandHandler(
    ICustomerRepository customerRepository,
    IServiceRequestRepository serviceRequestRepository,
    ITechnicianProfileRepository technicianProfileRepository,
    IAssignmentRepository assignmentRepository,
    ICurrentUser currentUser)
    : ICommandHandler<AssignServiceRequestCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        AssignServiceRequestCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's customer profile. The request
        //    owner is the actor allowed to assign their request; the identity
        //    is derived from the current user, never from the client.
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        // 2. Load the service request.
        var serviceRequest = await serviceRequestRepository.GetByIdAsync(
            command.ServiceRequestId,
            cancellationToken);

        if (serviceRequest is null)
        {
            return ServiceRequestErrors.NotFound;
        }

        // 3. Verify the request belongs to the current customer. An un-owned
        //    request is indistinguishable from a non-existent one, so request
        //    existence is never leaked.
        if (serviceRequest.CustomerProfileId != customerProfile.Id)
        {
            return ServiceRequestErrors.NotFound;
        }

        // 4. Load the selected technician with their services.
        var technicianProfile = await technicianProfileRepository.GetByIdWithServicesAsync(
            command.TechnicianProfileId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        // 5. The technician must provide the requested service category. This
        //    is the same eligibility rule already used by the accept flow.
        var providesCategory = technicianProfile.Services
            .Any(service => service.ServiceCategoryId == serviceRequest.ServiceCategoryId);

        if (!providesCategory)
        {
            return TechnicianProfileErrors.ServiceCategoryNotProvided;
        }

        // 6. Assign through the domain model: only a request currently
        //    searching for a technician can be assigned (SearchingTechnician
        //    -> Assigned). Accepted, completed, or cancelled requests are
        //    rejected here, which also prevents assigning a request twice.
        var assignResult = serviceRequest.Assign();

        if (assignResult.IsError)
        {
            return assignResult.Errors;
        }

        // 7. Create the pending assignment awaiting the technician's
        //    response. The assignment is created in the Pending state; the
        //    technician later responds through the existing accept/reject
        //    operations.
        var assignmentResult = Assignment.Create(
            Guid.NewGuid(),
            serviceRequest.Id,
            technicianProfile.Id);

        if (assignmentResult.IsError)
        {
            return assignmentResult.Errors;
        }

        // 8. Persist both changes (committed by the transaction pipeline).
        //    Optimistic concurrency (PostgreSQL xmin row version) prevents
        //    two concurrent assignment attempts from both succeeding: the
        //    second request conflicts on the ServiceRequest row version.
        serviceRequestRepository.Update(serviceRequest);

        await assignmentRepository.AddAsync(
            assignmentResult.Value,
            cancellationToken);

        return Result.Success;
    }
}
