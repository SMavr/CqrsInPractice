using CSharpFunctionalExtensions;
using System;

namespace Logic.Students
{
    public class Messages
    {
        private readonly IServiceProvider provider;

        public Messages(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public Result Dispatch(ICommand command)
        {
            Type type = typeof(ICommandHandler<>);
            Type[] typeArgs = { command.GetType() };

            // ICommandHandler<EditPersonalInfoCommand>
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = provider.GetService(handlerType);
            Result result = handler.Handle((dynamic)command);

            return result;
        }
    }
}
