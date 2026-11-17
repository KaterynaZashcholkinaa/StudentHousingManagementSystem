using Microsoft.Identity.Client;
using Student_Housing_Management_System.Models;
using Student_Housing_Management_System.Models.Enums;
using Student_Housing_Management_System.Repositories;
using Student_Housing_Management_System.ViewModels;

namespace Student_Housing_Management_System.Services
{
    public class ApplicationService
    {
        private readonly ApplicationRepository _applicationRepository;
        private readonly StudentRepository _studentRepository;
        private readonly RoomRepository _roomRepository;
        private readonly BuildingRepository _buildingRepository;

        public ApplicationService(
            ApplicationRepository applicationRepository,
            StudentRepository studentRepository,
            RoomRepository roomRepository,
            BuildingRepository buildingRepository)
        {
            _applicationRepository = applicationRepository;
            _studentRepository = studentRepository;
            _roomRepository = roomRepository;
            _buildingRepository = buildingRepository;
        }

        public List<ApplicationViewModel> GetAll()
        {
            var applications = _applicationRepository.GetAll();
            var students = _studentRepository.GetAll();
            var rooms = _roomRepository.GetAll();
            var buildings = _buildingRepository.GetAll();

            return applications.Select(a =>
            {
                var student = students.FirstOrDefault(s =>
                    s.StudentId == a.StudentId);

                var room = rooms.FirstOrDefault(r =>
                    r.RoomId == a.RoomId);

                var building = room == null ? null :
                    buildings.FirstOrDefault(b =>
                        b.BuildingId == room.BuildingId);

                return new ApplicationViewModel
                {
                    ApplicationId = a.ApplicationId,
                    StudentFirstName = student?.FirstName ?? string.Empty,
                    StudentLastName = student?.LastName ?? string.Empty,
                    RoomNumber = room?.RoomNumber ?? string.Empty,
                    BuildingName = building?.Name ?? string.Empty,
                    Address = building?.Address ?? string.Empty,
                    ApplicationDate = a.ApplicationDate,
                    ApplicationStatus = a.ApplicationStatus
                };
            }).ToList();
        }

        public ApplicationViewModel? GetById(int id)
        {
            var application = _applicationRepository.GetById(id);

            if (application == null)
            {
                return null;
            }

            var student = _studentRepository.GetById(application.StudentId);
            var room = _roomRepository.GetById(application.RoomId);

            var building = room == null ? null :
                _buildingRepository.GetById(room.BuildingId);

            return new ApplicationViewModel
            {
                ApplicationId = application.ApplicationId,
                StudentId = application.StudentId,
                RoomId = application.RoomId,
                StudentFirstName = student?.FirstName ?? string.Empty,
                StudentLastName = student?.LastName ?? string.Empty,
                RoomNumber = room?.RoomNumber ?? string.Empty,
                BuildingName = building?.Name ?? string.Empty,
                Address = building?.Address ?? string.Empty,
                ApplicationDate = application.ApplicationDate,
                ApplicationStatus = application.ApplicationStatus
            };
        }

        public bool Add(Application application)
        {
            var student = _studentRepository.GetById(application.StudentId);
            var room = _roomRepository.GetById(application.RoomId); 

            if (student == null || room == null)
            {
                return false;
            }

            application.ApplicationDate = DateOnly.FromDateTime(DateTime.Today);

            _applicationRepository.Add(application);

            return true;
        }


        public bool UpdateStatus(int applicationId, ApplicationStatus status)
        {
            return _applicationRepository.UpdateStatus(applicationId, status);
        }

        public bool Delete(int applicationId)
        {
            return _applicationRepository.Delete(applicationId);
        }
    }
}
