using Student_Housing_Management_System.Models.Enums;
namespace Student_Housing_Management_System.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int Floor { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public int BuildingId { get; set; }
        public Building? Building { get; set; }
    }
}
