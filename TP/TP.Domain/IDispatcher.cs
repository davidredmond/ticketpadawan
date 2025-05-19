using TP.Domain.Commands;
using TP.Domain.Queries;

namespace TP.Domain
{
    public interface IDispatcher
    {
        Task<TResult> SendCommandAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>;
        Task<TResult> SendQueryAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}
