using Microsoft.AspNetCore.Mvc;
using StudentApp.Data;
using StudentApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace StudentApp.Controllers
{
    [Authorize]
    public class ClassController : Controller
    {
        private readonly ClassService _service;

        public ClassController(ClassService service)
        {
            _service = service;
        }

        public IActionResult Index(string? search)
        {
            var classes = _service.GetAllClass(search);
            ViewBag.SearchQuery = search;
            return View(classes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Class @class)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _service.Add(@class);
                    TempData["Success"] = "Thêm lớp học thành công!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi: " + ex.Message);
                }
            }

            return View(@class);
        }

        // GET: /Class/Edit/5
        public IActionResult Edit(int id)
        {
            var classItem = _service.GetById(id);
            if (classItem == null)
                return NotFound();
            return View(classItem);
        }

        // POST: /Class/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Class @class)
        {
            if (id != @class.ClassId) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _service.Update(@class);
                    TempData["Success"] = "Cập nhật lớp học thành công!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Lỗi: " + ex.Message);
                }
            }

            return View(@class);
        }

        // GET: /Class/Delete/5
        public IActionResult Delete(int id)
        {
            var classItem = _service.GetById(id);
            if (classItem == null)
                return NotFound();

            return View(classItem);
        }

        // POST: /Class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _service.Delete(id);
                TempData["Success"] = "Xóa lớp học thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi xóa lớp: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
