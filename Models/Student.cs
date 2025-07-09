namespace StudentApp.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderType Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string RollNumber { get; set; }
        public string AdmissionNo { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string? Religion { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public int? CurrentClassId { get; set; }
        public StudentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
