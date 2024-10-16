using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeeker.Models
{
    public class Job
    {
        public int Id { get; set; }

        [ForeignKey("JobCategory")]
        public int? JobCategoryId { get; set; }

        [ForeignKey("JobPosition")]
        public int? JobPositionId { get; set; }

        [ForeignKey("Recruiter")]
        public int? RecruiterId { get; set; }

        [ForeignKey("Organization")]
        public int? OrganizationId { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Title { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Description { get; set; }

        public DateTime? PublishedAt { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Status { get; set; }

        [Column(TypeName = "TEXT")]
        public string? ImageUrl { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public JobPosition? JobPosition { get; set; }

        public JobCategory? JobCategory { get; set; }

        public Recruiter? Recruiter { get; set; }

        public Organization? Organization { get; set; }

        public ICollection<Applicant>? Applicants { get; set; }
    }
}