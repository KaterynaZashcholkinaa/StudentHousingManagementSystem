using Student_Housing_Management_System.Models.Enums;
namespace Student_Housing_Management_System.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public int RoomId { get; set; }
        public Room? Room { get; set; }
        public DateOnly ApplicationDate { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
    }
}
