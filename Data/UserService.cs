using Npgsql;
using StudentApp.Models;
using BCrypt.Net;
using System.Diagnostics.Eventing.Reader;

namespace StudentApp.Data
{
    public class UserService
    {
        private readonly string _connectionString;
        public UserService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public bool Register(User user)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            user.PasswordHash = passwordHash;

            var cmd = new NpgsqlCommand(@"INSERT INTO users (username, password_hash, email, role)
             VALUES (@username, @password, @email, @role::user_role)", conn);


            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@password", user.PasswordHash);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@role", user.Role.ToString());

            try
            {
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (PostgresException pgEx) when (pgEx.SqlState == "23505")
            {
                Console.WriteLine("Username hoặc email đã tồn tại.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi không xác định khi đăng ký: " + ex.Message);
                return false;
            }
        }

        public User? Authenticate(string username, string password)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT * FROM users WHERE username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string storedHash = reader.GetString(reader.GetOrdinal("password_hash"));
                if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                {
                    return new User
                    {
                        UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                        Username = reader.GetString(reader.GetOrdinal("username")),
                        Email = reader.GetString(reader.GetOrdinal("email")),
                        Role = Enum.Parse<UserRole>(reader.GetString(reader.GetOrdinal("role"))),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
                    };
                }
            }
            return null;
        }
    }
}