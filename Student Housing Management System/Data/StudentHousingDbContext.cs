using Microsoft.EntityFrameworkCore;
using Student_Housing_Management_System.Models;

namespace Student_Housing_Management_System.Data
{
    public class StudentHousingDbContext : DbContext
    {
        public StudentHousingDbContext(DbContextOptions<StudentHousingDbContext> options)
            : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Application> Applications { get; set; }
    }
}