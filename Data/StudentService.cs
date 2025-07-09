using Npgsql;
using StudentApp.Models;
using System.Data;

namespace StudentApp.Data
{
    public class StudentService
    {
        private readonly string _connectionString;

        public StudentService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public List<Class> GetAllClasses()
        {
            var classes = new List<Class>();
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT class_id, class_name FROM classes";
            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                classes.Add(new Class
                {
                    ClassId = reader.GetInt32(reader.GetOrdinal("class_id")),
                    ClassName = reader.GetString(reader.GetOrdinal("class_name"))
                });
            }

            return classes;
        }
        public List<Student> GetAll(string? search = null)
        {
            var students = new List<Student>();
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM Students";
            if (!string.IsNullOrWhiteSpace(search))
                sql += " WHERE first_name ILIKE @search OR last_name ILIKE @search";

            using var cmd = new NpgsqlCommand(sql, conn);
            if (!string.IsNullOrWhiteSpace(search))
                cmd.Parameters.AddWithValue("@search", $"%{search}%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                students.Add(new Student
                {
                    StudentId = reader.GetInt32(reader.GetOrdinal("student_id")),
                    FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                    LastName = reader.GetString(reader.GetOrdinal("last_name")),
                    Gender = Enum.Parse<GenderType>(reader.GetString(reader.GetOrdinal("gender"))),
                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                    RollNumber = reader.GetString(reader.GetOrdinal("roll_number")),
                    AdmissionNo = reader.GetString(reader.GetOrdinal("admission_no")),
                    AdmissionDate = reader.GetDateTime(reader.GetOrdinal("admission_date")),
                    Religion = reader["religion"] as string,
                    Address = reader["address"] as string,
                    PhoneNumber = reader["phone_number"] as string,
                    PhotoUrl = reader["photo_url"] as string,
                    Status = Enum.Parse<StudentStatus>(reader.GetString(reader.GetOrdinal("status")))
                });
            }
            return students;
        }

        public void Add(Student student)
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                string sql = @"
            INSERT INTO students (
                first_name, last_name, gender, date_of_birth, roll_number, admission_no,
                admission_date, religion, address, phone_number, photo_url,
                current_class_id, status, created_at
            )
            VALUES (
                @first_name, @last_name, @gender::gender_type, @dob, @roll, @admission_no,
                @admission_date, @religion, @address, @phone, @photo,
                @class_id, @status::student_status, @created_at
            );";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@first_name", student.FirstName);
                cmd.Parameters.AddWithValue("@last_name", student.LastName);
                cmd.Parameters.AddWithValue("@gender", NpgsqlTypes.NpgsqlDbType.Unknown, student.Gender.ToString());
                cmd.Parameters.AddWithValue("@dob", student.DateOfBirth);
                cmd.Parameters.AddWithValue("@roll", student.RollNumber);
                cmd.Parameters.AddWithValue("@admission_no", student.AdmissionNo);
                cmd.Parameters.AddWithValue("@admission_date", student.AdmissionDate);
                cmd.Parameters.AddWithValue("@religion", (object?)student.Religion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@address", (object?)student.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@phone", (object?)student.PhoneNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@photo", (object?)student.PhotoUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@class_id", (object?)student.CurrentClassId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", NpgsqlTypes.NpgsqlDbType.Unknown, student.Status.ToString());
                cmd.Parameters.AddWithValue("@created_at", student.CreatedAt);

                cmd.ExecuteNonQuery();
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                // Ném lỗi tùy chỉnh để controller xử lý
                throw new Exception("Admission No đã tồn tại trong hệ thống.");
            }
        }

        public void Delete(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            string sql = "DELETE FROM Students WHERE student_id = @id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public Student? GetById(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            string sql = "SELECT * FROM Students WHERE student_id = @id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Student
                {
                    StudentId = reader.GetInt32(reader.GetOrdinal("student_id")),
                    FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                    LastName = reader.GetString(reader.GetOrdinal("last_name")),
                    Gender = Enum.Parse<GenderType>(reader.GetString(reader.GetOrdinal("gender"))),
                    DateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                    RollNumber = reader.GetString(reader.GetOrdinal("roll_number")),
                    AdmissionNo = reader.GetString(reader.GetOrdinal("admission_no")),
                    AdmissionDate = reader.GetDateTime(reader.GetOrdinal("admission_date")),
                    Religion = reader["religion"] as string,
                    Address = reader["address"] as string,
                    PhoneNumber = reader["phone_number"] as string,
                    PhotoUrl = reader["photo_url"] as string,
                    CurrentClassId = reader["current_class_id"] as int?,
                    Status = Enum.Parse<StudentStatus>(reader.GetString(reader.GetOrdinal("status")))
                };
            }
            return null;
        }
    }
}