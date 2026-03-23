using Microsoft.AspNetCore.Identity;

namespace TechWorld.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public virtual ICollection<UserGame> UserWishlist { get; set; } 
            = new List<UserGame>();
    }
}
