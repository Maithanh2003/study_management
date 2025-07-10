using System.ComponentModel.DataAnnotations;
using StudentApp.Models;

namespace StudentApp.DTO.Requests
{
    public class StudentCreateRequest
    {
        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [Required]
        public GenderType Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string RollNumber { get; set; } = "";

        [Required]
        public string AdmissionNo { get; set; } = "";

        [Required]
        [DataType(DataType.Date)]
        public DateTime AdmissionDate { get; set; }

        public string? Religion { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string? PhotoUrl { get; set; }

        public int? CurrentClassId { get; set; }

        [Required]
        public StudentStatus Status { get; set; }
    }
}
