using Microsoft.AspNetCore.Mvc;
using StudentApp.Data;
using StudentApp.Models;

namespace StudentApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService2 _userService;

        public UserController(UserService2 userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Details");
        }


        // GET: /User/Details/5
        public IActionResult Details()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login");

            var user = _userService.GetById(userId.Value);
            if (user == null)
                return NotFound();

            return View("~/Views/Account/Details.cshtml", user);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View("~/Views/Account/ChangePassword.cshtml");
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid) return View("~/Views/Account/ChangePassword.cshtml");

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var user = _userService.GetById(userId.Value);
            if (user == null) return NotFound();
            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.PasswordHash))
            {
                ViewBag.Message = "Mật khẩu hiện tại không đúng.";
                return View("~/Views/Account/ChangePassword.cshtml", model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                ViewBag.Message = "Mật khẩu mới không khớp.";
                return View("~/Views/Account/ChangePassword.cshtml", model);
            }

            _userService.UpdatePassword(user.UserId, model.NewPassword);

            ViewBag.Message = "Đổi mật khẩu thành công!";
            return View("~/Views/Account/Details.cshtml", user);
        }
    }
}
