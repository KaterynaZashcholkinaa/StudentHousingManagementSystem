using Student_Housing_Management_System.Models.Enums;

namespace Student_Housing_Management_System.ViewModels
{
    public class ApplicationViewModel
    {
        public int ApplicationId { get; set; }
        public string StudentFirstName { get; set; } = string.Empty;
        public string StudentLastName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public string BuildingName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateOnly ApplicationDate { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
    }
}
