using CSharpFunctionalExtensions;
using Logic.Utils;

namespace Logic.Students
{
    public interface ICommand
    {
    }

    public interface IQuery<TResult>
    {
    }

    public interface ICommandHandler<TCommand>
       where TCommand : ICommand
    {
        Result Handle(TCommand command);
    }

    public interface IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query); 
    }

    public class EditPersonalInfoCommand : ICommand
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public sealed class EditPersonalInfoCommandHandler : ICommandHandler<EditPersonalInfoCommand>
    {
        private readonly UnitOfWork unitOfWork;

        public EditPersonalInfoCommandHandler(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Result Handle(EditPersonalInfoCommand command)
        {
            var repository = new StudentRepository(unitOfWork);
            Student student = repository.GetById(command.Id);
            if (student == null)
                return Result.Fail($"No student found for Id {command.Id}");

            student.Name = command.Name;
            student.Email = command.Email;

            unitOfWork.Commit();

            return Result.Ok();
        }   
    }
}
