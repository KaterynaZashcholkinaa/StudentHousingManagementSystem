using Microsoft.EntityFrameworkCore;
using Student_Housing_Management_System.Data;
using Student_Housing_Management_System.Models;

namespace Student_Housing_Management_System.Repositories
{
    public class BuildingRepository
    {
        private readonly StudentHousingDbContext _context;

        public BuildingRepository(StudentHousingDbContext context)
        {
            _context = context;
        }

        public List<Building> GetAll()
        {
            return _context.Buildings
                .Include(b => b.Rooms)
                .ToList();
        }

        public Building? GetById(int id)
        {
            return _context.Buildings
                .Include(b => b.Rooms)
                .FirstOrDefault(b => b.BuildingId == id);
        }

        public void Add(Building building)
        {
            _context.Buildings.Add(building);
            _context.SaveChanges();
        }

        public bool Update(Building building)
        {
            Building? existingBuilding = GetById(building.BuildingId);

            if (existingBuilding == null)
            {
                return false;
            }

            existingBuilding.Name = building.Name;
            existingBuilding.Address = building.Address;

            _context.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            Building? building = GetById(id);

            if (building == null || building.Rooms.Any())
            {
                return false;
            }

            _context.Buildings.Remove(building);
            _context.SaveChanges();

            return true;
        }
    }
}
