using Microsoft.AspNetCore.Identity;

namespace NewBlog.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }

        public IEnumerable<Role> Roles => UserRoles.Select(ur => ur.Role);
    }
}
