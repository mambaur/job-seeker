

using System.Data;
using Dapper;
using JobSeeker.Models;
using JobSeeker.Utilities;
using MySql.Data.MySqlClient;

namespace JobSeeker.Repositories
{
    public class SeekerRepository
    {
        private readonly string? connectionString;

        public SeekerRepository()
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<User> GetAllSeekers()
        {
            using IDbConnection  db = new MySqlConnection(connectionString);
            return db.Query<User>("SELECT * FROM Users WHERE UserRoleId='3'");
        }

        public void Store(User user)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "INSERT INTO Users (UserRoleId, Name, Email, Username, Password, ImageUrl) VALUES ('3', @Name, @Email, @Username, '123456', @ImageUrl)";
            db.Execute(sql, user);
        }

        public bool Update(User User)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "UPDATE Users SET Name = @Name, Email = @Email, Username = @Username, ImageUrl = @ImageUrl WHERE Id = @Id";
            var result = db.Execute(sql, User);
            return result > 0;
        }

        public User? GetSeekerById(int Id)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            return db.Query<User>($"SELECT * FROM Users WHERE Id = '{Id}'").FirstOrDefault();
        }

        public bool DeleteSeekerById(int OrganizationId)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            int rowsAffected = db.Execute("DELETE FROM Users WHERE Id = @Id", new { Id = OrganizationId });
            return rowsAffected > 0;
        }
    }
}