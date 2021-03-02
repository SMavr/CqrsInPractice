using CSharpFunctionalExtensions;
using Logic.Students;
using System;

namespace Logic.Utils
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

        public T Dispatch<T>(IQuery<T> query)
        {
            Type type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = provider.GetService(handlerType);
            T result = handler.Handle((dynamic)query);

            return result;
        }
    }
}
