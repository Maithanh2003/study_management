using Microsoft.EntityFrameworkCore;
using StudentApp.DTO.Requests;
using StudentApp.Models;
using StudentApp.Utils;

namespace StudentApp.Data
{
    public class StudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Lấy tất cả lớp học
        public List<Class> GetAllClasses()
        {
            return _context.Classes
                .OrderBy(c => c.ClassName)
                .ToList();
        }
        // ✅ Lấy tất cả sinh viên (có tìm kiếm)
        public PagedResult<Student> GetAll(string? search = null, int pageNumber = 1, int pageSize = 5)
        {
            var query = _context.Students
                .Include(s => s.CurrentClass)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    EF.Functions.ILike(s.FirstName, $"%{search}%") ||
                    EF.Functions.ILike(s.LastName, $"%{search}%"));
            }

            int totalItems = query.Count();

            var students = query
                .OrderBy(s => s.FirstName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<Student>
            {
                Items = students,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = pageNumber
            };
        }

        public void Add(StudentCreateRequest request)
        {
            var student = new Student
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                RollNumber = request.RollNumber,
                AdmissionNo = request.AdmissionNo,
                AdmissionDate = request.AdmissionDate,
                Religion = request.Religion,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                PhotoUrl = request.PhotoUrl,
                CurrentClassId = request.CurrentClassId,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                _context.Students.Add(student);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("admission_no") == true)
            {
                throw new Exception("Admission No đã tồn tại trong hệ thống.");
            }
        }
        // ✅ Lấy sinh viên theo ID
        public Student? GetById(int id)
        {
            return _context.Students
                .Include(s => s.CurrentClass)
                .FirstOrDefault(s => s.StudentId == id);
        }

        // ✅ Xoá sinh viên
        public void Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
        }

        // ✅ Cập nhật sinh viên
        public void Update(StudentUpdateRequest request)
        {
            var student = _context.Students.Find(request.StudentId);
            if (student == null) return;

            student.FirstName = request.FirstName;
            student.LastName = request.LastName;
            student.Gender = request.Gender;
            student.DateOfBirth = request.DateOfBirth;
            student.RollNumber = request.RollNumber;
            student.AdmissionNo = request.AdmissionNo;
            student.AdmissionDate = request.AdmissionDate;
            student.Religion = request.Religion;
            student.Address = request.Address;
            student.PhoneNumber = request.PhoneNumber;
            student.PhotoUrl = request.PhotoUrl;
            student.CurrentClassId = request.CurrentClassId;
            student.Status = request.Status;

            _context.SaveChanges();
        }

    }
}
