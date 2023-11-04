namespace ProjectSemantic3WebMVC.Models
{
    public enum Gender
    {
        Male,
        Female,
    }

    public enum AcademicDegree
    {
        None,
        Bachleor,
        Master,
        Doctor
    }

    public class PersonModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }

        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Descriptions { get; set; }

        public string PhotoURL { get; set; }
    }
}