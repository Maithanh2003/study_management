
using StudentApp.Models;
using Microsoft.EntityFrameworkCore;
using StudentApp.Utils;

namespace StudentApp.Data
{
    public class ClassService
    {
        private readonly AppDbContext _context;

        public ClassService(AppDbContext context)
        {
            _context = context;
        }

        public PagedResult<Class> GetAllClass(string? search = null, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Classes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.ClassName.ToLower().Contains(search.ToLower()));
            }
            int totalItems = query.Count();
            var classes = query
            .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<Class>
            {
                Items = classes,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = pageNumber
            };
        }

        public Class? GetById(int id)
        {
            return _context.Classes.Find(id);
        }

        public void Add(Class @class)
        {
            _context.Classes.Add(@class);
            _context.SaveChanges();
        }

        public void Update(Class @class)
        {
            _context.Classes.Update(@class);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.Classes.Find(id);
            if (entity != null)
            {
                _context.Classes.Remove(entity);
                _context.SaveChanges();
            }
        }

        public List<Student> GetStudentsByClassId(int classId)
        {
            return _context.Students
                .Include(s => s.CurrentClass)
                .Where(s => s.CurrentClassId == classId)
                .OrderBy(s => s.FirstName)
                .ToList();
        }
    }
}