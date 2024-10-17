
using System.Data;
using Dapper;
using JobSeeker.Models;
using JobSeeker.Utilities;
using MySql.Data.MySqlClient;

namespace JobSeeker.Repositories
{
    public class RoleRepository
    {
        private readonly string? connectionString;

        public RoleRepository()
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<UserRole> GetAllRoles()
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            return db.Query<UserRole>("SELECT * FROM UserRoles");
        }

        public void Store(UserRole role)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "INSERT INTO UserRoles (Name) VALUES (@Name)";
            db.Execute(sql, role);
        }

        public bool Update(UserRole UserRole)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "UPDATE UserRoles SET Name = @Name WHERE Id = @Id";
            var result = db.Execute(sql, UserRole);
            return result > 0;
        }

        public UserRole? GetRoleById(int Id)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            return db.Query<UserRole>($"SELECT * FROM UserRoles WHERE Id = '{Id}'").FirstOrDefault();
        }

        public bool DeleteRoleById(int roleId)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            int rowsAffected = db.Execute("DELETE FROM UserRoles WHERE Id = @Id", new { Id = roleId });
            return rowsAffected > 0;
        }
    }
}