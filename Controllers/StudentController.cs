using Microsoft.AspNetCore.Mvc;
using StudentApp.Data;
using StudentApp.Models;
using StudentApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace StudentApp.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {

        private readonly StudentService _service;

        public StudentController(StudentService service)
        {
            _service = service;
        }

        public IActionResult Index(string? search)
        {
            var students = _service.GetAll(search);
            return View(students);
        }

        // GET: /Student/Create
        public IActionResult Create()
        {
            var viewModel = new StudentFormViewModel
            {
                Student = new Student(),
                Classes = _service.GetAllClasses() // Phải trả về List<Class> không null
            };
            return View(viewModel);
        }


        // POST: /Student/Create
        [HttpPost]
        public IActionResult Create(StudentFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    viewModel.Student.CreatedAt = DateTime.Now;
                    _service.Add(viewModel.Student);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            // Nếu lỗi, cần load lại danh sách lớp
            viewModel.Classes = _service.GetAllClasses();
            return View(viewModel);
        }

        // GET /Student/Delete/5
        public IActionResult Delete(int id)
        {
            var student = _service.GetById(id);
            if (student == null) return NotFound();
            return View(student);
        }

        // POST /Student/DeleteConfirmed
        [HttpPost("Student/DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int studentId)
        {
            if (studentId == 0)
                return BadRequest("Thiếu thông tin StudentId");

            _service.Delete(studentId);
            return RedirectToAction("Index");
        }


        public IActionResult Details(int id)
        {
            var student = _service.GetById(id);
            if (student == null) return NotFound();
            return View(student);
        }
    }
}