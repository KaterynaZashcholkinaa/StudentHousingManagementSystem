
using Student_Housing_Management_System.Data;
using Student_Housing_Management_System.Models;
using Student_Housing_Management_System.Models.Enums;

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

        public Application? GetById(int applicationId)
        {
            return _context.Applications
                .FirstOrDefault(a => a.ApplicationId == applicationId);
        }

        public void Add(Application application)
        {
            _context.Applications.Add(application);
            _context.SaveChanges();
        }

        public bool UpdateStatus(int applicationId, ApplicationStatus status)
        {
            Application? existingApplication = GetById(applicationId);

            if (existingApplication == null)
            {
                return false;
            }

            existingApplication.ApplicationStatus = status;

            _context.SaveChanges();

            return true;
        }

        public bool Delete(int applicationId)
        {
            Application? application = GetById(applicationId);

            if (application == null)
            {
                return false;
            }

            _context.Applications.Remove(application);
            _context.SaveChanges();

            return true;
        }
    }
}