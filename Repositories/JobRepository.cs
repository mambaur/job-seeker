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
    public class JobRepository
    {
        private readonly string? connectionString;

        public JobRepository()
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // public IEnumerable<Job> GetAllJobs()
        // {
        //     using IDbConnection  db = new MySqlConnection(connectionString);
        //     return db.Query<Job>("SELECT * FROM Jobs");
        // }

        public IEnumerable<Job> GetAllJobs()
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = @"
                SELECT j.Id, j.Title, j.Description, j.Status, j.ImageUrl,
                        o.Id, o.Name, o.ImageUrl,
                        jc.Id, jc.Name,
                        u.Id, u.Name, u.ImageUrl
                FROM Jobs j
                INNER JOIN Organizations o ON o.Id = j.OrganizationId
                INNER JOIN JobCategories jc ON jc.Id = j.JobCategoryId
                INNER JOIN Users u ON u.Id = j.RecruiterId
            ";
            return db.Query<Job, Organization, JobCategory, User, Job>(sql, (job, organization, jobCategory, user) =>
            {
                job.Organization = organization;
                job.JobCategory = jobCategory;
                job.Recruiter = user;
                return job;
            }, splitOn: "Id, Id, Id").ToList();
        }

        public void Store(Job job)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "INSERT INTO Jobs (JobCategoryId, JobPositionId, RecruiterId, OrganizationId, Title, Description, PublishedAt, StartDate, EndDate, Status, ImageUrl) VALUES (@JobCategoryId, @JobPositionId, @RecruiterId, @OrganizationId, @Title, @Description, @PublishedAt, @StartDate, @EndDate, @Status, @ImageUrl)";
            db.Execute(sql, job);
        }

        public bool Update(Job Job)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "UPDATE Jobs SET JobCategoryId = @JobCategoryId, JobPositionId = @JobPositionId, RecruiterId = @RecruiterId, OrganizationId = @OrganizationId, Title = @Title, Description = @Description, PublishedAt = @PublishedAt, StartDate = @StartDate, EndDate = @EndDate, Status = @Status, ImageUrl = @ImageUrl WHERE Id = @Id";
            var result = db.Execute(sql, Job);
            return result > 0;
        }

        public Job? GetJobById(int Id)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            return db.Query<Job>($"SELECT * FROM Jobs WHERE Id = '{Id}'").FirstOrDefault();
        }

        public bool DeleteJobById(int JobId)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            int rowsAffected = db.Execute("DELETE FROM Jobs WHERE Id = @Id", new { Id = JobId });
            return rowsAffected > 0;
        }
    }
}