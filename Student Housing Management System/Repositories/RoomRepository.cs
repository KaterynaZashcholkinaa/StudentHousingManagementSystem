
using Student_Housing_Management_System.Data;
using Student_Housing_Management_System.Models;

namespace Student_Housing_Management_System.Repositories
{
    public class RoomRepository
    {
        private readonly StudentHousingDbContext _context;

        public RoomRepository(StudentHousingDbContext context)
        {
            _context = context;
        }

        public List<Room> GetAll()
        {
            return _context.Rooms.ToList();
        }

        public Room? GetById(int id)
        {
            return _context.Rooms
                .FirstOrDefault(r => r.RoomId == id);
        }
    }
}
