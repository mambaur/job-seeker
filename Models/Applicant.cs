
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeeker.Models
{
    public class Applicant
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }

        [ForeignKey("Job")]
        public int? JobId { get; set; }

        public DateTime? AppliedAt { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

    }
}