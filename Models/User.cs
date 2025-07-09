// namespace StudentApp.Models
// {
//     public class User
//     {
//         public int UserId { get; set; }
//         public string Username { get; set; }
//         public string PasswordHash { get; set; }
//         public string Email { get; set; }
//         public UserRole Role { get; set; }
//         public DateTime CreatedAt { get; set; }
//     }
// }
using System.ComponentModel.DataAnnotations.Schema;
using StudentApp.Models;

using System.ComponentModel.DataAnnotations.Schema;

namespace StudentApp.Models
{
    [Table("users")]
    public class User
    {
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("role", TypeName = "user_role")]
        public UserRole Role { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
