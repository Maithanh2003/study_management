// using Npgsql;
// using StudentApp.Models;
// using System.Data;

// namespace StudentApp.Data
// {
//     public class UserService2
//     {
//         private readonly string _connectionString;
//         public UserService2(IConfiguration configuration)
//         {
//             _connectionString = configuration.GetConnectionString("DefaultConnection");
//         }

//         public User? GetById(int id)
//         {
//             using var conn = new NpgsqlConnection(_connectionString);
//             conn.Open();

//             string sql = "SELECT * FROM users WHERE user_id = @id";
//             using var cmd = new NpgsqlCommand(sql, conn);
//             cmd.Parameters.AddWithValue("@id", id);

//             using var reader = cmd.ExecuteReader();

//             if (reader.Read())
//             {
//                 return new User
//                 {
//                     UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
//                     Username = reader.GetString(reader.GetOrdinal("username")),
//                     PasswordHash = reader.IsDBNull(reader.GetOrdinal("password_hash")) ? null : reader.GetString(reader.GetOrdinal("password_hash")),
//                     Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email")),
//                     Role = Enum.Parse<UserRole>(reader.GetString(reader.GetOrdinal("role"))),
//                     CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))

//                 };
//             }

//             return null;
//         }
//         public void UpdatePassword(int userId, string newPassword)
//         {
//             using var conn = new NpgsqlConnection(_connectionString);
//             conn.Open();
//             string passwordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
//             string sql = "UPDATE users SET password_hash = @password WHERE user_id = @id";
//             using var cmd = new NpgsqlCommand(sql, conn);
//             cmd.Parameters.AddWithValue("@password", passwordHash);
//             cmd.Parameters.AddWithValue("@id", userId);
//             cmd.ExecuteNonQuery();
//         }

//     }
// }

using StudentApp.Models;

namespace StudentApp.Data
{
    public class UserService2
    {
        private readonly AppDbContext _context;

        public UserService2(AppDbContext context)
        {
            _context = context;
        }

        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void UpdatePassword(int userId, string newPassword)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _context.SaveChanges();
        }
    }
}
