using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeeker.Models
{
    public class Recruiter
    {
        public int Id { get; set; }

        [ForeignKey("Organization")]
        public int? OrganizationId { get; set; }

        [StringLength(191)]
        public string? Name { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Description { get; set; }

        [Column(TypeName = "TEXT")]
        public string? ImageUrl { get; set; }

        public Organization? Organization{ get; set; }
    }
}