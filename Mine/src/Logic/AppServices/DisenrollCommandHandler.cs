using CSharpFunctionalExtensions;
using Logic.Students;
using Logic.Utils;

namespace Logic.AppServices
{
    public sealed class DisenrollCommandHandler : ICommandHandler<DisenrollCommand>
    {
        private readonly UnitOfWork unitOfWork;

        public DisenrollCommandHandler(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Result Handle(DisenrollCommand command)
        {
            var repository = new StudentRepository(unitOfWork);
            Student student = repository.GetById(command.Id);
            if (student == null)
                return Result.Fail($"No student found for Id {command.Id}");

            if (string.IsNullOrWhiteSpace(command.Comment))
                return Result.Fail("Disenrollment comment is required");

            Enrollment enrollment = student.GetEnrollment(command.EnrollmentNumber);
            if (enrollment == null)
                return Result.Fail($"No enrollment fount with number '{enrollment}'");

            student.RemoveEnrollment(enrollment, command.Comment);

            unitOfWork.Commit();

            return Result.Ok();
        }
    }

}
