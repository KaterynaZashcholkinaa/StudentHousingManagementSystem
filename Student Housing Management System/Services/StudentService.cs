
using Student_Housing_Management_System.Models;
using Student_Housing_Management_System.Repositories;

namespace Student_Housing_Management_System.Services
{
    public class StudentService
    {
        private readonly StudentRepository _studentRepository;

        public StudentService(StudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public List<Student> GetAll()
        {
            return _studentRepository.GetAll();
        }

        public Student? GetById(int id)
        {
            return _studentRepository.GetById(id);
        }

        public void Add(Student student)
        {
            _studentRepository.Add(student);
        }

        public bool Update(Student student)
        {
            return _studentRepository.Update(student);
        }

        public bool Delete(int id)
        {
            return _studentRepository.Delete(id);
        }
    }
}
    