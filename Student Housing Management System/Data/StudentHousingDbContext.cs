using Microsoft.EntityFrameworkCore;

namespace Student_Housing_Management_System.Data
{
    public class StudentHousingDbContext : DbContext
    {
        public StudentHousingDbContext(DbContextOptions<StudentHousingDbContext> options)
            : base(options)
        {
        }
    }
}