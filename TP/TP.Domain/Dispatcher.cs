using Microsoft.Extensions.DependencyInjection;
using TP.Domain.Commands;
using TP.Domain.Queries;

namespace TP.Domain
{
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider _provider;

        public Dispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<TResult> SendCommandAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>
        {
            var handler = _provider.GetService<ICommandHandler<TCommand, TResult>>();

            if (handler == null)
            {
                throw new InvalidOperationException($"No handler registered for {typeof(TCommand).Name}");
            }

            return await handler.HandleAsync(command);
        }

        public async Task<TResult> SendQueryAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            var handler = _provider.GetService<IQueryHandler<TQuery, TResult>>();

            if (handler == null)
            {
                throw new InvalidOperationException($"No handler registered for {typeof(TQuery).Name}");
            }

            return await handler.HandleAsync(query);
        }
    }
}
