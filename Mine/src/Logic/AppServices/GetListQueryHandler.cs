using CSharpFunctionalExtensions;
using Dapper;
using Logic.Dtos;
using Logic.Students;
using Logic.Utils;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Logic.AppServices
{
    public sealed class GetListQueryHandler : IQueryHandler<GetListQuery, List<StudentDto>>
    {
        private readonly ConnectionString connectionString;

        public GetListQueryHandler(ConnectionString connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<StudentDto> Handle(GetListQuery query)
        {
            string sql = @"
                    SELECT s.StudentID Id, s.Name, s.Email,
	                    s.FirstCourseName Course1, s.FirstCourseCredits Course1Credits, s.FirstCourseGrade Course1Grade,
	                    s.SecondCourseName Course2, s.SecondCourseCredits Course2Credits, s.SecondCourseGrade Course2Grade
                    FROM dbo.Student s
                    WHERE (s.FirstCourseName = @Course
		                    OR s.SecondCourseName = @Course
		                    OR @Course IS NULL)
                        AND (s.NumberOfEnrollments = @Number
                            OR @Number IS NULL)
                    ORDER BY s.StudentID ASC";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString.Value))
            {
                List<StudentInDb> students = sqlConnection
                    .Query<StudentInDb>(sql, new
                    {
                        Course = query.EnrolledIn,
                        Number = query.NumberOfCourses
                    })
                    .ToList();

                return null;
            }
        }

        private StudentDto ConvertToDto(Student student)
        {
            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Course1 = student.FirstEnrollment?.Course?.Name,
                Course1Grade = student.FirstEnrollment?.Grade.ToString(),
                Course1Credits = student.FirstEnrollment?.Course?.Credits,
                Course2 = student.SecondEnrollment?.Course?.Name,
                Course2Grade = student.SecondEnrollment?.Grade.ToString(),
                Course2Credits = student.SecondEnrollment?.Course?.Credits,
            };
        }
    }

    public class StudentInDb
    {
        public StudentInDb(long studentId, string name, string email,
            Grade? grade, string courseName, int? credits)
        {
            StudentId = studentId;
            Name = name;
            Email = email;
            Grade = grade;
            CourseName = courseName;
            Credits = credits;
        }

        public long StudentId { get; }
        public string Name { get; }
        public string Email { get; }
        public Grade? Grade { get; }
        public string CourseName { get; }
        public int? Credits { get; }
    }

}
