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
    public class JobPositionRepository
    {
        private readonly string? connectionString;

        public JobPositionRepository()
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<JobPosition> GetAllJobPositions()
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            return db.Query<JobPosition>("SELECT * FROM JobPositions");
        }

        public void Store(JobPosition job)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "INSERT INTO JobPositions (Name, Description) VALUES (@Name, @Description)";
            db.Execute(sql, job);
        }

        public bool Update(JobPosition JobPosition)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "UPDATE JobPositions SET Name = @Name, Description = @Description WHERE Id = @Id";
            var result = db.Execute(sql, JobPosition);
            return result > 0;
        }

        public JobPosition? GetJobPositionById(int Id)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            return db.Query<JobPosition>($"SELECT * FROM JobPositions WHERE Id = '{Id}'").FirstOrDefault();
        }

        public bool DeleteJobPositionById(int JobPositionId)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            int rowsAffected = db.Execute("DELETE FROM JobPositions WHERE Id = @Id", new { Id = JobPositionId });
            return rowsAffected > 0;
        }
    }
}