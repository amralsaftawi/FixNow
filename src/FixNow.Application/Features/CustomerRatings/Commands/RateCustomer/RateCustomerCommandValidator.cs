using FluentValidation;

namespace FixNow.Application.Features.CustomerRatings.Commands.RateCustomer;

public sealed class RateCustomerCommandValidator
    : AbstractValidator<RateCustomerCommand>
{
    public RateCustomerCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty()
            .WithErrorCode("Job.IdRequired");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithErrorCode("CustomerRating.InvalidRating");
    }
}
