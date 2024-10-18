using System.Data;
using Dapper;
using JobSeeker.Models;
using JobSeeker.Utilities;
using MySql.Data.MySqlClient;

namespace JobSeeker.Repositories
{
    public class ApplicantRepository
    {
        private readonly string? connectionString;

        public ApplicantRepository()
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // public IEnumerable<Applicant> GetAllApplicants()
        // {
        //     using IDbConnection db = new MySqlConnection(connectionString);
        //     return db.Query<Applicant>("SELECT * FROM Applicants");
        // }

        public IEnumerable<Applicant> GetAllApplicants()
        {
            using IDbConnection db = new MySqlConnection(connectionString);

            var sql = @"
                SELECT a.Id, a.Status, a.AppliedAt,
                        u.Id, u.Name, u.ImageUrl,
                        j.Id, j.Title
                FROM Applicants a
                INNER JOIN Users u ON u.Id = a.UserId
                INNER JOIN Jobs j ON j.Id = a.JobId
            ";
            return db.Query<Applicant, User, Job, Applicant>(sql, (applicant, user, job) =>
            {
                applicant.User = user;
                applicant.Job = job;
                return applicant;
            }, splitOn: "Id, Id").ToList();
        }

        public void Store(Applicant applicant)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "INSERT INTO Applicants (UserId, JobId, AppliedAt, Status) VALUES (@UserId, @JobId, @AppliedAt, @Status)";
            db.Execute(sql, applicant);
        }

        public bool Update(Applicant Applicant)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            var sql = "UPDATE Applicants SET UserId = @UserId, JobId = @JobId, Status = @Status WHERE Id = @Id";
            var result = db.Execute(sql, Applicant);
            return result > 0;
        }

        public Applicant? GetApplicantById(int Id)
        {
            using IDbConnection db = new MySqlConnection(connectionString);

            var sql = $@"
                SELECT a.Id, a.UserId, a.JobId, a.Status, a.AppliedAt,
                        u.Id, u.Name, u.ImageUrl,
                        j.Id, j.Title
                FROM Applicants a
                INNER JOIN Users u ON u.Id = a.UserId
                INNER JOIN Jobs j ON j.Id = a.JobId
                WHERE a.Id = '{Id}'
            ";
            return db.Query<Applicant, User, Job, Applicant>(sql, (applicant, user, job) =>
            {
                applicant.User = user;
                applicant.Job = job;
                return applicant;
            }, splitOn: "Id, Id").FirstOrDefault();
        }

        public bool DeleteApplicantById(int ApplicantId)
        {
            using IDbConnection db = new MySqlConnection(connectionString);
            int rowsAffected = db.Execute("DELETE FROM Applicants WHERE Id = @Id", new { Id = ApplicantId });
            return rowsAffected > 0;
        }
    }
}