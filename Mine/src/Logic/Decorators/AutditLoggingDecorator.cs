using CSharpFunctionalExtensions;
using Logic.Students;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Decorators
{
    public sealed class AutditLoggingDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> handler;

        public AutditLoggingDecorator(ICommandHandler<TCommand> handler)
        {
            this.handler = handler;
        }

        public Result Handle(TCommand command)
        {
            string commandJson = JsonConvert.SerializeObject(command);

            // Use proper logging
            Console.WriteLine($"Command of type {command.GetType().Name}: {commandJson}");

            return handler.Handle(command);
        }
    }
}
