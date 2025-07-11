using StudentApp.DTO.Requests;
using StudentApp.Models;

namespace StudentApp.Models.ViewModels
{
    public class StudentEditViewModel
    {
        public required StudentUpdateRequest Student { get; set; }
        public List<Class> Classes { get; set; } = new();
        public IFormFile? Photo { get; set; }
    }

}
