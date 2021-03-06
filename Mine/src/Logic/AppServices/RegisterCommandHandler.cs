﻿using CSharpFunctionalExtensions;
using Logic.Students;
using Logic.Utils;
using System;

namespace Logic.AppServices
{
    public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand>
    {
        private readonly UnitOfWork unitOfWork;

        public RegisterCommandHandler(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Result Handle(RegisterCommand command)
        {
            var courseRepository = new CourseRepository(unitOfWork);
            var studentRepository = new StudentRepository(unitOfWork);

            var student = new Student(command.Name, command.Email);

            if (command.Course1 != null && command.Course1Grade != null)
            {
                Course course = courseRepository.GetByName(command.Course1);
                student.Enroll(course, Enum.Parse<Grade>(command.Course1Grade));
            }

            if (command.Course2 != null && command.Course2Grade != null)
            {
                Course course = courseRepository.GetByName(command.Course2);
                student.Enroll(course, Enum.Parse<Grade>(command.Course2Grade));
            }

            studentRepository.Save(student);
            unitOfWork.Commit();

            return Result.Ok();
        }
    }

}
