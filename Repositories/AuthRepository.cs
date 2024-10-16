using System.Data;
using Dapper;
using JobSeeker.Models;
using JobSeeker.Utilities;
using MySql.Data.MySqlClient;

namespace JobSeeker.Repositories
{
    public class AuthRepository
    {
        private readonly string? connectionString;

        public AuthRepository()
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public User? GetUserByUsername(string username)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            return db.Query<User>("SELECT * FROM Users WHERE Username = @Username", new { Username = username }).FirstOrDefault();
        }

        public void RegisterUser(User user)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "INSERT INTO Users (Username, Name, Password, Email, UserRoleId) VALUES (@Username, @Name, @Password, @Email, '1')";
            db.Execute(sql, user);
        }

        public User? AuthenticateUser(string username, string password)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            return db.Query<User>($"SELECT * FROM Users WHERE Username = '{username}' AND Password = '{password}' AND UserRoleId = '1'",
                new { Username = username, Password = password }).FirstOrDefault();
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "UPDATE Users SET Username = @Username, Email = @Email, Name = @Name WHERE Id = @Id";
            var result = await db.ExecuteAsync(sql, user);
            return result > 0;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string newPassword)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = $"UPDATE Users SET Password = '{newPassword}' WHERE Id = '{userId}'"; // Ganti dengan nama tabel dan kolom Anda
            var result = await db.ExecuteAsync(sql, new { Password = newPassword, Id = userId });
            return result > 0;
        }
    }
}