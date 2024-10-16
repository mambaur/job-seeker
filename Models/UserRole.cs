
namespace JobSeeker.Models
{
    public class UserRole
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}