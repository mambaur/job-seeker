using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JobSeeker.Models
{
    public class JobPosition
    {
        public int Id { get; set; }

        [StringLength(191)]
        public string? Name { get; set; }

        [Column(TypeName = "TEXT")]
        public string? Description { get; set; }

    }
}