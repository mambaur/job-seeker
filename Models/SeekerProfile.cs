using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeeker.Models
{
    public class SeekerProfile
    {
        public int Id { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Address { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Education { get; set; }

        public string? Contact { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Headline { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Position { get; set; }

        [Column(TypeName = "TEXT")]
        public string? About { get; set; }
    }
}