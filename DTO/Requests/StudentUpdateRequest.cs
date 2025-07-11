using StudentApp.Models;

namespace StudentApp.DTO.Requests
{
    public class StudentUpdateRequest
    {
        public int StudentId { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public GenderType Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string RollNumber { get; set; } = string.Empty;
        public string AdmissionNo { get; set; } = string.Empty;
        public DateTime AdmissionDate { get; set; }

        public string? Religion { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }

        public int? CurrentClassId { get; set; }
        public StudentStatus Status { get; set; }
    }
}
