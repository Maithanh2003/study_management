namespace StudentApp.Models.ViewModels
{
    public class StudentFormViewModel
    {
        public Student Student { get; set; }
        public List<Class> Classes { get; set; } = new();
    }
}
