
using Student_Housing_Management_System.Data;
using Student_Housing_Management_System.Models;

namespace Student_Housing_Management_System.Repositories
{
    public class ApplicationRepository
    {
        private readonly StudentHousingDbContext _context;

        public ApplicationRepository(StudentHousingDbContext context)
        {
            _context = context;
        }

        public List<Application> GetAll()
        {
            return _context.Applications.ToList();
        }

        public Application? GetById(int id)
        {
            return _context.Applications
                .FirstOrDefault(a => a.ApplicationId == id);
        }
    }
}
