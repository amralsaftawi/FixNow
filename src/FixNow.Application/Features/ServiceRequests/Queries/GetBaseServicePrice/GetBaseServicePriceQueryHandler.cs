using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.ServiceRequests.Queries.GetBaseServicePrice;

public sealed class GetBaseServicePriceQueryHandler(
    ICustomerRepository customerRepository,
    IServiceRequestRepository serviceRequestRepository,
    IAssignmentRepository assignmentRepository,
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : IQueryHandler<GetBaseServicePriceQuery, Result<GetBaseServicePriceResponse>>
{
    public async Task<Result<GetBaseServicePriceResponse>> Handle(
        GetBaseServicePriceQuery query,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's customer profile. This is
        //    also the customer-only authorization gate: a user without a
        //    customer profile cannot request a base service price.
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        // 2. Resolve the base service price and inspection fee from the
        //    existing pricing source of truth (the service request's
        //    service category). The ownership check is applied inside the
        //    database query, so a service request belonging to another
        //    customer is indistinguishable from a non-existent one. The
        //    client can never supply or influence the prices: they are
        //    always resolved by the server from the domain.
        var basePrice = await serviceRequestRepository.GetBaseServicePriceAsync(
            query.ServiceRequestId,
            customerProfile.Id,
            cancellationToken);

        if (basePrice is null)
        {
            return ServiceRequestErrors.NotFound;
        }

        // 3. When the request already has an accepted technician, resolve
        //    that technician's specific price for the category and prefer it
        //    over the category base price. The technician-specific price is
        //    only applied for the technician actually engaged on this
        //    request; other technicians' prices never influence the quote.
        var resolvedBasePrice = basePrice.BasePrice;

        var assignment = await assignmentRepository.GetAcceptedByRequestAsync(
            query.ServiceRequestId,
            cancellationToken);

        if (assignment is not null)
        {
            var technicianPrice =
                await technicianProfileRepository.GetServicePriceByCategoryAsync(
                    assignment.TechnicianProfileId,
                    basePrice.ServiceCategoryId,
                    cancellationToken);

            if (technicianPrice is not null)
            {
                resolvedBasePrice = technicianPrice;
            }
        }

        return new GetBaseServicePriceResponse(
            ServiceRequestId: query.ServiceRequestId,
            ServiceCategoryId: basePrice.ServiceCategoryId,
            ServiceCategoryName: basePrice.ServiceCategoryName,
            BasePrice: resolvedBasePrice,
            InspectionFee: basePrice.InspectionFee);
    }
}
