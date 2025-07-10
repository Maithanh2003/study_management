using StudentApp.DTO.Requests;

namespace StudentApp.Models.ViewModels
{
    public class StudentFormViewModel
    {
        // public Student Student { get; set; }
        // public List<Class> Classes { get; set; } = new();
        public StudentCreateRequest Student { get; set; } = new();
        public List<Class> Classes { get; set; } = new();
    }
}
