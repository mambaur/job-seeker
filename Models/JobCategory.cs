
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSeeker.Models
{
    public class JobCategory
    {
        public int Id { get; set; }

        [StringLength(191)]
        public string? Name { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Description { get; set; }

        public ICollection<Job>? Jobs{ get; set; }
    }
}