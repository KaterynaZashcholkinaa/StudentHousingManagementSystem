
using Student_Housing_Management_System.Models;
using Student_Housing_Management_System.Repositories;

namespace Student_Housing_Management_System.Services
{
    public class BuildingService
    {
        private readonly BuildingRepository _buildingRepository;

        public BuildingService(BuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository;
        }

        public List<Building> GetAll()
        {
            return _buildingRepository.GetAll();
        }

        public Building? GetById(int id)
        {
            return _buildingRepository.GetById(id);
        }

        public void Add(Building building)
        {
            _buildingRepository.Add(building);
        }

        public bool Update(Building building)
        {
            return _buildingRepository.Update(building);
        }

        public bool Delete(int id)
        {
            return _buildingRepository.Delete(id);
        }
    }
}
