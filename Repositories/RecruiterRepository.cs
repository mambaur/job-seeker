
using System.Data;
using Dapper;
using JobSeeker.Models;
using JobSeeker.Utilities;
using MySql.Data.MySqlClient;

namespace JobSeeker.Repositories
{
    public class RecruiterRepository
    {
        private readonly string? connectionString;

        public RecruiterRepository()
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<User> GetAllRecruiters()
        {
            using IDbConnection  db = new MySqlConnection(connectionString);
            return db.Query<User>("SELECT * FROM Users WHERE UserRoleId='2'");
        }

        public void Store(User user)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "INSERT INTO Users (UserRoleId, Name, Email, Username, Password, ImageUrl) VALUES ('2', @Name, @Email, @Username, '123456', @ImageUrl)";
            db.Execute(sql, user);
        }

        public bool Update(User User)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "UPDATE Users SET Name = @Name, Email = @Email, Username = @Username, ImageUrl = @ImageUrl WHERE Id = @Id";
            var result = db.Execute(sql, User);
            return result > 0;
        }

        public User? GetRecruiterById(int Id)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            return db.Query<User>($"SELECT * FROM Users WHERE Id = '{Id}'").FirstOrDefault();
        }

        public bool DeleteRecruiterById(int RecruiterId)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            int rowsAffected = db.Execute("DELETE FROM Users WHERE Id = @Id", new { Id = RecruiterId });
            return rowsAffected > 0;
        }
    }
}