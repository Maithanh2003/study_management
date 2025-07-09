using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StudentApp.Data;
using StudentApp.Models;
using StudentApp.Utils;

namespace StudentApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly JwtHelper _jwtHelper;

        public AccountController(UserService userService, JwtHelper jwtHelper)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
        }

        // GET: /Account/Login
        public IActionResult Login() => View();

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _userService.Authenticate(username, password);
            if (user == null)
            {
                ViewBag.Message = "Sai tên đăng nhập hoặc mật khẩu";
                return View();
            }

            var token = _jwtHelper.GenerateToken(user);
            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetInt32("UserId", user.UserId);
            return RedirectToAction("Index", "Student");
        }

        // GET: /Account/Register
        public IActionResult Register() => View();

        // POST: /Account/Register
        [HttpPost]
        public IActionResult Register(string username, string email, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(role))
            {
                ViewBag.Message = "Vui lòng điền đầy đủ thông tin.";
                return View();
            }

            try
            {
                var newUser = new User
                {
                    Username = username.Trim(),
                    Email = email.Trim(),
                    PasswordHash = password, // plaintext, sẽ được hash trong service
                    Role = Enum.Parse<UserRole>(role.ToUpper()),
                    CreatedAt = DateTime.Now
                };

                bool result = _userService.Register(newUser);
                if (!result)
                {
                    ViewBag.Message = "Tên đăng nhập hoặc email đã tồn tại.";
                    return View();
                }

                TempData["Success"] = "Đăng ký thành công! Hãy đăng nhập.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Đã xảy ra lỗi: " + ex.Message;
                return View();
            }
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
