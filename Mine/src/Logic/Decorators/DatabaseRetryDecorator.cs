using CSharpFunctionalExtensions;
using Logic.Students;
using Logic.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Decorators
{
    public sealed class DatabaseRetryDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> handler;
        private readonly Config config;

        public DatabaseRetryDecorator(ICommandHandler<TCommand> handler, Config config)
        {
            this.handler = handler;
            this.config = config;
        }

        public Result Handle(TCommand command)
        {
            for (int i = 0; i < config.NumberOfDatabaseRetries; i++)
            {
                try
                {
                    Result result = handler.Handle(command);
                    return result;
                }
                catch (Exception ex)
                {
                    if (i >= config.NumberOfDatabaseRetries || !IsDatabaseException(ex))
                        throw;
                }
            }

            throw new InvalidOperationException("Should not ever get here");
        }

        private bool IsDatabaseException(Exception exception)
        {
            string message = exception.InnerException?.Message;

            if (message == null)
                return false;

            return message.Contains("The connection is broken and recovery in not possible")
                || message.Contains("error occurred while establishing a connection");
        }
    }
}
