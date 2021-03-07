using CSharpFunctionalExtensions;
using Logic.Students;
using Logic.Utils;
using System;

namespace Logic.AppServices
{
    public sealed class TransferCommandHandler : ICommandHandler<TransferCommand>
    {
        private readonly UnitOfWork unitOfWork;

        public TransferCommandHandler(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Result Handle(TransferCommand command)
        {
            var studentRepository = new StudentRepository(unitOfWork);
            var courseRepository = new CourseRepository(unitOfWork);

            Student student = studentRepository.GetById(command.Id);
            if (student == null)
                return Result.Fail($"No student found for Id {command.Id}");

            Course course = courseRepository.GetByName(command.Course);
            if (course == null)
                return Result.Fail($"Course is incorrect: {command.Course}");

            bool success = Enum.TryParse(command.Grade, out Grade grade);
            if (!success)
                return Result.Fail($"Grade is incorrect: {command.Grade}");

            Enrollment enrollment = student.GetEnrollment(command.EnrollmentNumber);
            if (enrollment == null)
                return Result.Fail($"No enrollment fount with number '{enrollment}'");

            enrollment.Update(course, grade);

            unitOfWork.Commit();

            return Result.Ok();
        }
    }

}
