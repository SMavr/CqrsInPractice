using CSharpFunctionalExtensions;
using Logic.Students;
using Logic.Utils;
using System;

namespace Logic.AppServices
{
    public sealed class EnrollCommandHandler : ICommandHandler<EnrollCommand>
    {
        private readonly UnitOfWork unitOfWork;

        public EnrollCommandHandler(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public Result Handle(EnrollCommand command)
        {
            var courseRepository = new CourseRepository(unitOfWork);
            var studentRepository = new StudentRepository(unitOfWork);

            Student student = studentRepository.GetById(command.Id);
            if (student == null)
                return Result.Fail($"No student found for Id {command.Id}");

            Course course = courseRepository.GetByName(command.Course);
            if (course == null)
                return Result.Fail($"Course is incorrect: {command.Course}");

            bool success = Enum.TryParse(command.Grade, out Grade grade);
            if (!success)
                return Result.Fail($"Grade is incorrect: {command.Grade}");

            student.Enroll(course, grade);

            unitOfWork.Commit();

            return Result.Ok();
        }
    }

}
