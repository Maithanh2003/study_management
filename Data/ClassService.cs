using Npgsql;
using StudentApp.Models;
using System.Data;

namespace StudentApp.Data
{
    public class ClassService
    {
        private readonly string _connectionString;

        public ClassService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DeFaultConnection");
        }

        public List<Class> GetAllClass(string? search = null)
        {
            var classes = new List<Class>();
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT class_id, class_name, description FROM classes";
            if (!string.IsNullOrWhiteSpace(search))
            {
                sql += " WHERE LOWER(class_name) LIKE @search";
            }
            using var cmd = new NpgsqlCommand(sql, conn);
            if (!string.IsNullOrWhiteSpace(search))
            {
                cmd.Parameters.AddWithValue("@search", $"%{search.ToLower()}%");
            }
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                classes.Add(new Class
                {
                    ClassId = reader.GetInt32(reader.GetOrdinal("class_id")),
                    ClassName = reader.GetString(reader.GetOrdinal("class_name")),
                    Description = reader.GetString(reader.GetOrdinal("description"))
                });
            }
            return classes;
        }

        public void Add(Class @class)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                string sql = "INSERT INTO classes (class_name, description) VALUES (@class_name, @description);";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@class_name", @class.ClassName);
                cmd.Parameters.AddWithValue("@description", (object?)@class.Description ?? DBNull.Value);
                cmd.ExecuteNonQuery();

            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public Class? GetById(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            string sql = "SELECT class_id, class_name, description FROM classes WHERE class_id = @id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Class
                {
                    ClassId = reader.GetInt32(reader.GetOrdinal("class_id")),
                    ClassName = reader.GetString(reader.GetOrdinal("class_name")),
                    Description = reader.IsDBNull("description") ? null : reader.GetString(reader.GetOrdinal("description"))
                };
            }
            return null;
        }

        public void Update(Class @class)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            string sql = "UPDATE classes SET class_name = @class_name, description = @description WHERE class_id = @id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@class_name", @class.ClassName);
            cmd.Parameters.AddWithValue("@description", (object?)@class.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@id", @class.ClassId);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            string sql = "DELETE FROM classes WHERE class_id = @id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

    }
}