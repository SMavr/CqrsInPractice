using CSharpFunctionalExtensions;

namespace Logic.AppServices
{
    public interface ICommandHandler<TCommand>
       where TCommand : ICommand
    {
        Result Handle(TCommand command);
    }

}
