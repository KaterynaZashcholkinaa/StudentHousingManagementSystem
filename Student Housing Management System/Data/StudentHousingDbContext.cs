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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .Property(s => s.StudentId)
                .HasColumnName("Id");

            modelBuilder.Entity<Student>()
                .Property(s => s.PhoneNumber)
                .HasColumnName("Phone");


            modelBuilder.Entity<Building>()
                .Property(b => b.BuildingId)
                .HasColumnName("Id");


            modelBuilder.Entity<Room>()
                .Property(r => r.RoomId)
                .HasColumnName("Id");

            modelBuilder.Entity<Room>()
                .Property(r => r.RoomStatus)
                .HasColumnName("Status");

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Building)
                .WithMany()
                .HasForeignKey(r => r.BuildingId);


            modelBuilder.Entity<Application>()
                .Property(a => a.ApplicationId)
                .HasColumnName("Id");

            modelBuilder.Entity<Application>()
                .Property(a => a.ApplicationStatus)
                .HasColumnName("Status");

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentId);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Room)
                .WithMany()
                .HasForeignKey(a => a.RoomId);
        }
    }
}