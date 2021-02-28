namespace Logic.Students
{
    public class EditPersonalInfoCommand
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public sealed class EditPersonalInfoCommandHandler
    {
        public void Handle(EditPersonalInfoCommand command)
        {

            //Student student = _studentRepository.GetById(id);
            //if (student == null)
            //    return Error($"No student found for Id {id}");

            //student.Name = dto.Name;
            //student.Email = dto.Email;

            //_unitOfWork.Commit();
        }
    }
}
