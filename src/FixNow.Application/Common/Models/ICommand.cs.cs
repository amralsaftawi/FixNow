using MediatR;

namespace FixNow.Application.Common.Abstractions.Messaging;

public interface ICommand<out TResponse>
    : IRequest<TResponse>;