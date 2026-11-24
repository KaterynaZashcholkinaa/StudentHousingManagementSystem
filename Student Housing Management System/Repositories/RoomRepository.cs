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

        public List<Room> GetByBuildingId(int buildingId)
        {
            return _context.Rooms
                .Where(r => r.BuildingId == buildingId)
                .ToList();
        }

        public Room? GetById(int id)
        {
            return _context.Rooms
                .FirstOrDefault(r => r.RoomId == id);
        }

        public void Add(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
        }

        public bool Update(Room room)
        {
            Room? existingRoom = GetById(room.RoomId);

            if (existingRoom == null)
            {
                return false;
            }

            existingRoom.RoomNumber = room.RoomNumber;
            existingRoom.Floor = room.Floor;
            existingRoom.Price = room.Price;
            existingRoom.RoomStatus = room.RoomStatus;
            existingRoom.BuildingId = room.BuildingId;

            _context.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            Room? room = GetById(id);

            if (room == null)
            {
                return false;
            }

            bool hasApplications = _context.Applications
                .Any(a => a.RoomId == id);

            if (hasApplications)
            {
                return false;
            }

            _context.Rooms.Remove(room);
            _context.SaveChanges();

            return true;
        }
    }
}
