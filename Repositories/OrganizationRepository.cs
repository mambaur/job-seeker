using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using JobSeeker.Models;
using JobSeeker.Utilities;
using MySql.Data.MySqlClient;

namespace JobSeeker.Repositories
{
    public class OrganizationRepository
    {
        private readonly string? connectionString;

        public OrganizationRepository()
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Organization> GetAllOrganizations()
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            return db.Query<Organization>("SELECT * FROM Organization");
        }

        public void Store(Organization organization)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "INSERT INTO Organization (Name, Description, ImageUrl) VALUES (@Name, @Description, @ImageUrl)";
            db.Execute(sql, organization);
        }

        public bool Update(Organization Organization)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "UPDATE Organization SET Name = @Name, Description = @Description, ImageUrl = @ImageUrl WHERE Id = @Id";
            var result = db.Execute(sql, Organization);
            return result > 0;
        }

        public Organization? GetOrganizationById(int Id)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            return db.Query<Organization>($"SELECT * FROM Organization WHERE Id = '{Id}'").FirstOrDefault();
        }

        public bool DeleteOrganizationById(int OrganizationId)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            int rowsAffected = db.Execute("DELETE FROM Organization WHERE Id = @Id", new { Id = OrganizationId });
            return rowsAffected > 0;
        }
    }
}