using Microsoft.EntityFrameworkCore;
using Student_Housing_Management_System.Data;
using Student_Housing_Management_System.Models;

namespace Student_Housing_Management_System.Repositories
{
    public class StudentRepository
    {
        private readonly StudentHousingDbContext _context;

        public StudentRepository(StudentHousingDbContext context)
        {
            _context = context;
        }

        public List<Student> GetAll()
        {
            return _context.Students.ToList();
        }

        public Student? GetById(int id)
        {
            return _context.Students.FirstOrDefault(s => s.StudentId == id);
        }
    }
}