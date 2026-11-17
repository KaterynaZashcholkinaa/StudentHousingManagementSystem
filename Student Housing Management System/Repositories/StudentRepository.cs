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

        public void Add(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public bool Update(Student student)
        {
            Student? existingStudent = GetById(student.StudentId);

            if (existingStudent == null)
            {
                return false;
            }

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.Email = student.Email;
            existingStudent.PhoneNumber = student.PhoneNumber;

            _context.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            Student? student = GetById(id);

            if (student == null)
            {
                return false;
            }

            _context.Students.Remove(student);
            _context.SaveChanges();

            return true;
        }
    }
}