using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceCategories.Commands.ConfigureServiceCategoryInspectionFee;

public sealed record ConfigureServiceCategoryInspectionFeeCommand(
    Guid ServiceCategoryId,
    decimal Amount,
    Currency Currency)
    : ICommand<Result<Updated>>;
