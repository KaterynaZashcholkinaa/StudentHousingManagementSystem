
using Student_Housing_Management_System.Models;
using Student_Housing_Management_System.Repositories;

namespace Student_Housing_Management_System.Services
{
    public class RoomService
    {
        private readonly RoomRepository _roomRepository;

        public RoomService(RoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public List<Room> GetAll()
        {
            return _roomRepository.GetAll();
        }

        public List<Room> GetByBuildingId(int buildingId)
        {
            return _roomRepository.GetByBuildingId(buildingId);
        }

        public Room? GetById(int id)
        {
            return _roomRepository.GetById(id);
        }

        public void Add(Room room)
        {
            _roomRepository.Add(room);
        }

        public bool Update(Room room)
        {
            return _roomRepository.Update(room);
        }

        public bool Delete(int id)
        {
            return _roomRepository.Delete(id);
        }
    }
}
    