using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeeker.Models
{
    public class User
    {
        public int Id { get; set; }

        [ForeignKey("UserRole")]
        public int? UserRoleId { get; set; }

        [StringLength(191)]
        [Required]
        public string? Name { get; set; }

        [StringLength(191)]
        [Required]
        public string? Email { get; set; }

        [StringLength(191)]
        public string? Username { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Password { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public ICollection<Applicant>? Applicants { get; set; }

        public UserRole? UserRole { get; set; }
    }
}