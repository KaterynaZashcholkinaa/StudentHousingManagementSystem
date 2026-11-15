
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
            return _context.Buildings.ToList();
        }

        public Building? GetById(int id)
        {
            return _context.Buildings
                .FirstOrDefault(b => b.BuildingId == id);
        }
    }
}
