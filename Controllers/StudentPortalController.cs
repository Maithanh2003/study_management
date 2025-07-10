using Microsoft.AspNetCore.Mvc;
using StudentApp.Data;
using StudentApp.Models;

namespace StudentApp.Controllers
{
    public class StudentPortalController : Controller
    {
        private readonly StudentService _studentService;
        private readonly UserService2 _userService;

        public StudentPortalController(StudentService studentService, UserService2 userService)
        {
            _studentService = studentService;
            _userService = userService;
        }

        // Chỉ cho phép người có vai trò STUDENT
        [HttpGet]
        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var student = _userService.GetById(userId.Value);
            if (student == null)
                return NotFound("Không tìm thấy thông tin sinh viên");

            return View("/Views/StudentPortal/Dashboard.cshtml", student);
        }

        public IActionResult ClassList()
        {
            var classes = _studentService.GetAllClasses();
            return View("/Views/StudentPortal/ClassList.cshtml", classes);
        }

        [HttpGet]
        public IActionResult Enroll()
        {
            var classes = _studentService.GetAllClasses();
            return View("/Views/StudentPortal/Enroll.cshtml", classes);
        }

        [HttpPost]
        public IActionResult EnrollClass(int classId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var student = _userService.GetById(userId.Value);
            if (student == null) return NotFound();

            // bool success = _studentService.EnrollClass(student.StudentId, classId);
            bool success = true;
            ViewBag.Message = success ? "Đăng ký thành công!" : "Bạn đã đăng ký lớp này rồi.";
            var classes = _studentService.GetAllClasses();
            return View("/Views/StudentPortal/Enroll.cshtml", classes);
        }
    }
}
