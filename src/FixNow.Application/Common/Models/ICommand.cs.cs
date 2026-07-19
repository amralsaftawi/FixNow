using MediatR;

namespace FixNow.Application.Common.Models;

public interface ICommand<TResponse> : IRequest<TResponse>
{
}