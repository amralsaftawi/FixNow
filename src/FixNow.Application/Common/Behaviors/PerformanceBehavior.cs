using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;


public sealed class PerformanceBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private const long SlowRequestThresholdInMilliseconds = 500;

    private readonly ILogger<TRequest> _logger;

    public PerformanceBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            return await next();
        }
        finally
        {
            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds > SlowRequestThresholdInMilliseconds)
            {
                _logger.LogWarning(
                    "Slow request detected. Request: {RequestName}, Elapsed: {ElapsedMilliseconds} ms",
                    typeof(TRequest).Name,
                    stopwatch.ElapsedMilliseconds);
            }
        }
    }
}