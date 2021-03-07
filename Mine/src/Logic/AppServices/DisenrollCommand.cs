namespace Logic.AppServices
{
    public sealed class DisenrollCommand : ICommand
    {
        public DisenrollCommand(long id, int enrollmentNumber, string comment)
        {
            Id = id;
            EnrollmentNumber = enrollmentNumber;
            Comment = comment;
        }

        public long Id { get; }
        public int EnrollmentNumber { get; }
        public string Comment { get; }
    }

}
