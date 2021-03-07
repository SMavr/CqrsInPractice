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
        private readonly CommandConnectionString connectionString;

        public GetListQueryHandler(CommandConnectionString connectionString)
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

                List<long> ids = students
                    .GroupBy(x => x.StudentId)
                    .Select(x => x.Key)
                    .ToList();

                var result = new List<StudentDto>();

                foreach (var id in ids)
                {
                    List<StudentInDb> data = students
                        .Where(x => x.StudentId == id)
                        .ToList();

                    var dto = new StudentDto
                    {
                        Id = data[0].StudentId,
                        Name = data[0].Name,
                        Email = data[0].Email,
                        Course1 = data[0].CourseName,
                        Course1Credits = data[0].Credits,
                        Course1Grade = data[0]?.Grade.ToString()
                    };

                    if (data.Count > 1)
                    {
                        dto.Course2 = data[1].CourseName;
                        dto.Course2Credits = data[1].Credits;
                        dto.Course2Grade = data[1]?.Grade.ToString();
                    }

                    result.Add(dto);
                }

                return result;
            }
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
