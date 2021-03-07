using CSharpFunctionalExtensions;
using Logic.Students;
using Logic.Utils;

namespace Logic.AppServices
{
    public sealed class UnregisterCommandHandler : ICommandHandler<UnregisterCommand>
    {
        private readonly UnitOfWork unitOfWork;

        public UnregisterCommandHandler(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Result Handle(UnregisterCommand command)
        {
            var repository = new StudentRepository(unitOfWork);
            Student student = repository.GetById(command.Id);
            if (student == null)
                return Result.Fail($"No student found for Id {command.Id}");

            repository.Delete(student);
            unitOfWork.Commit();

            return Result.Ok();
        }
    }

}
