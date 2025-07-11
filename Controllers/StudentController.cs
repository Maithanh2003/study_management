using Microsoft.AspNetCore.Mvc;
using StudentApp.Data;
using Microsoft.AspNetCore.Authorization;
using StudentApp.DTO.Requests;
using System;
using StudentApp.Models.ViewModels;

namespace StudentApp.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {

        private readonly StudentService _service;
        private const int DEFAULT_PAGE_SIZE = 10;

        public StudentController(StudentService service)
        {
            _service = service;
        }
        // public IActionResult Index(StudentSearchRequest request)
        // {
        //     var result = _service.GetAll(request);

        //     ViewBag.SearchModel = request;
        //     ViewBag.Classes = _service.GetAllClasses();
        //     var routeValues = new Dictionary<string, string>
        //     {
        //     { "Keyword", request.Keyword ?? "" },
        //     { "Gender", request.Gender ?? "" },
        //     { "Religion", request.Religion ?? "" },
        //     { "ClassId", request.ClassId?.ToString() ?? "" },
        //     { "SortBy", request.SortBy ?? "" },
        //     { "IsDescending", request.IsDescending.ToString().ToLower() },
        //     { "PageSize", request.PageSize.ToString() }
        //     };


        //     ViewData["RouteValues"] = routeValues;
        //     return View(result);
        // }

        public IActionResult Index(string? search, int pageNumber = 1, int pageSize = 5)
        {
            var result = _service.GetAll(search, pageNumber, pageSize);

            // Lưu route value để truyền sang pagination
            var routeValues = new Dictionary<string, string>
        {
            { "search", search ?? "" },
            { "pageSize", pageSize.ToString() }
        };

            ViewData["RouteValues"] = routeValues;
            ViewData["Action"] = "Index";
            ViewData["SearchKeyword"] = search;

            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new StudentFormViewModel
            {
                Student = new StudentCreateRequest(),
                Classes = _service.GetAllClasses()
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Create(StudentFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Classes = _service.GetAllClasses();
                return View(viewModel);
            }

            if (viewModel.Photo != null && viewModel.Photo.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(viewModel.Photo.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Photo", "Chỉ cho phép các định dạng ảnh JPG, PNG, GIF.");
                    viewModel.Classes = _service.GetAllClasses();
                    return View(viewModel);
                }

                // Tạo đường dẫn lưu file
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder);

                // Tạo tên file duy nhất
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Lưu file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    viewModel.Photo.CopyTo(stream);
                }

                // Gán đường dẫn tương đối để lưu DB
                viewModel.Student.PhotoUrl = "/uploads/" + fileName;
            }

            try
            {
                _service.Add(viewModel.Student);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                viewModel.Classes = _service.GetAllClasses();
                return View(viewModel);
            }
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

        // GET: /Student/Edit/5
        public IActionResult Edit(int id)
        {
            var student = _service.GetById(id);
            if (student == null)
                return NotFound();

            var classes = _service.GetAllClasses();

            var viewModel = new StudentEditViewModel
            {
                Student = new StudentUpdateRequest
                {
                    StudentId = student.StudentId,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Gender = student.Gender,
                    DateOfBirth = student.DateOfBirth,
                    RollNumber = student.RollNumber,
                    AdmissionNo = student.AdmissionNo,
                    AdmissionDate = student.AdmissionDate,
                    Religion = student.Religion,
                    Address = student.Address,
                    PhoneNumber = student.PhoneNumber,
                    PhotoUrl = student.PhotoUrl,
                    CurrentClassId = student.CurrentClassId,
                    Status = student.Status
                },
                Classes = classes
            };

            return View(viewModel);
        }

        // POST: /Student/Edit
        [HttpPost]
        public IActionResult Edit(StudentEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Classes = _service.GetAllClasses();
                return View(viewModel);
            }
            var existingStudent = _service.GetById(viewModel.Student.StudentId);
            if (existingStudent == null)
                return NotFound();
            if (viewModel.Photo != null && viewModel.Photo.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(viewModel.Photo.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Photo", "Chỉ cho phép các định dạng ảnh JPG, PNG, GIF.");
                    viewModel.Classes = _service.GetAllClasses();
                    return View(viewModel);
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    viewModel.Photo.CopyTo(stream);
                }

                viewModel.Student.PhotoUrl = "/uploads/" + fileName;
            }
            else
            {
                viewModel.Student.PhotoUrl = existingStudent.PhotoUrl;
            }

            try
            {
                _service.Update(viewModel.Student);
                TempData["Success"] = "Cập nhật thông tin sinh viên thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi cập nhật: " + ex.Message);
                viewModel.Classes = _service.GetAllClasses();
                return View(viewModel);
            }
        }

    }
}