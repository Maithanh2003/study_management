using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentApp.Models
{
    [Table("students")]
    public class Student
    {
        [Key]
        [Column("student_id")]
        public int StudentId { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("first_name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("last_name")]
        public string LastName { get; set; }

        [Required]
        [Column("gender", TypeName = "varchar(20)")]
        public GenderType Gender { get; set; }

        [Required]
        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("roll_number")]
        public string RollNumber { get; set; }

        [Required]
        [Column("admission_date")]
        public DateTime AdmissionDate { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("admission_no")]
        public string AdmissionNo { get; set; }

        [MaxLength(50)]
        [Column("religion")]
        public string? Religion { get; set; }

        [MaxLength(255)]
        [Column("address")]
        public string? Address { get; set; }

        [MaxLength(20)]
        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        [MaxLength(255)]
        [Column("photo_url")]
        public string? PhotoUrl { get; set; }

        [Column("current_class_id")]
        public int? CurrentClassId { get; set; }

        [Required]
        [Column("status", TypeName = "varchar(20)")]
        public StudentStatus Status { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Optional: điều hướng quan hệ
        public User? User { get; set; }

        [ForeignKey("CurrentClassId")]
        public Class? CurrentClass { get; set; }
    }
}
