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
    public class JobCategoryRepository
    {
        private readonly string? connectionString;

        public JobCategoryRepository()
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<JobCategory> GetAllJobCategories()
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            return db.Query<JobCategory>("SELECT * FROM JobCategory");
        }

        public void Store(JobCategory job)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "INSERT INTO JobCategory (Name, Description) VALUES (@Name, @Description)";
            db.Execute(sql, job);
        }

        public bool Update(JobCategory JobCategory)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "UPDATE JobCategory SET Name = @Name, Description = @Description WHERE Id = @Id";
            var result = db.Execute(sql, JobCategory);
            return result > 0;
        }

        public JobCategory? GetJobCategoryById(int Id)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            return db.Query<JobCategory>($"SELECT * FROM JobCategory WHERE Id = '{Id}'").FirstOrDefault();
        }

        public bool DeleteJobCategoryById(int JobCategoryId)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            int rowsAffected = db.Execute("DELETE FROM JobCategory WHERE Id = @Id", new { Id = JobCategoryId });
            return rowsAffected > 0;
        }
    }
}