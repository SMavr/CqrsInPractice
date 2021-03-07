namespace Logic.AppServices
{
    public sealed class EnrollCommand : ICommand
    {
        public EnrollCommand(long id, string course, string grade)
        {
            Id = id;
            Course = course;
            Grade = grade;
        }

        public long Id { get; }
        public string Course { get; }
        public string Grade { get; }
    }

}
