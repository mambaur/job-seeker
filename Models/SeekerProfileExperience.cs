
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeeker.Models
{
    public class SeekerProfileExperience
    {
        public int Id { get; set; }

        public int? SeekerProfileId { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Title { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? IsCurrenct { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Description { get; set; }

        public string? Organization { get; set; }

        [Column(TypeName = "TEXT")]
        public string? OrganizationAddress { get; set; }

        public string? LocationType { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Skills { get; set; }
    }
}