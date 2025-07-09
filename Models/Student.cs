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
// using System.ComponentModel.DataAnnotations.Schema;

// namespace StudentApp.Models
// {
//     [Table("students")]
//     public class Student
//     {
//         [Column("student_id")]
//         public int StudentId { get; set; }

//         [Column("user_id")]
//         public int? UserId { get; set; }
//         public User? User { get; set; }

//         [Column("first_name")]
//         public string FirstName { get; set; }

//         [Column("last_name")]
//         public string LastName { get; set; }

//         [Column("gender", TypeName = "gender_type")]
//         public GenderType Gender { get; set; }

//         [Column("date_of_birth")]
//         public DateTime DateOfBirth { get; set; }

//         [Column("roll_number")]
//         public string RollNumber { get; set; }

//         [Column("admission_no")]
//         public string AdmissionNo { get; set; }

//         [Column("admission_date")]
//         public DateTime AdmissionDate { get; set; }

//         [Column("religion")]
//         public string? Religion { get; set; }

//         [Column("address")]
//         public string? Address { get; set; }

//         [Column("phone_number")]
//         public string? PhoneNumber { get; set; }

//         [Column("photo_url")]
//         public string? PhotoUrl { get; set; }

//         [Column("current_class_id")]
//         public int? CurrentClassId { get; set; }
//         public Class? CurrentClass { get; set; }

//         [Column("status", TypeName = "student_status")]
//         public StudentStatus Status { get; set; }

//         [Column("created_at")]
//         public DateTime CreatedAt { get; set; }
//     }
// }
