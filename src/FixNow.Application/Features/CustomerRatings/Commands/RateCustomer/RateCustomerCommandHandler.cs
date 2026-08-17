using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.CustomerRatings.Commands.RateCustomer;

public sealed class RateCustomerCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IJobRepository jobRepository,
    ICustomerRatingRepository customerRatingRepository,
    ICurrentUser currentUser)
    : ICommandHandler<RateCustomerCommand, Result<RateCustomerResponse>>
{
    public async Task<Result<RateCustomerResponse>> Handle(
        RateCustomerCommand command,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await technicianProfileRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        var job = await jobRepository.GetByIdAsync(
            command.JobId,
            cancellationToken);

        if (job is null)
        {
            return JobErrors.NotFound;
        }

        if (job.Status != JobStatus.Completed)
        {
            return CustomerRatingErrors.JobNotCompleted;
        }

        if (job.TechnicianProfileId != technicianProfile.Id)
        {
            return JobErrors.NotFound;
        }

        var jobAccess = await jobRepository.GetAccessAsync(
            command.JobId,
            cancellationToken);

        if (jobAccess is null)
        {
            return JobErrors.NotFound;
        }

        var alreadyRated = await customerRatingRepository.ExistsByJobIdAsync(
            command.JobId,
            cancellationToken);

        if (alreadyRated)
        {
            return CustomerRatingErrors.AlreadyRated;
        }

        var ratingResult = CustomerRatingScore.Create(command.Rating);

        if (ratingResult.IsError)
        {
            return ratingResult.Errors;
        }

        var customerRatingResult = CustomerRating.Create(
            id: Guid.NewGuid(),
            jobId: command.JobId,
            technicianProfileId: technicianProfile.Id,
            customerProfileId: jobAccess.ServiceRequestCustomerProfileId,
            rating: ratingResult.Value,
            comment: command.Comment);

        if (customerRatingResult.IsError)
        {
            return customerRatingResult.Errors;
        }

        await customerRatingRepository.AddAsync(
            customerRatingResult.Value,
            cancellationToken);

        return new RateCustomerResponse(
            CustomerRatingId: customerRatingResult.Value.Id,
            Rating: command.Rating);
    }
}
