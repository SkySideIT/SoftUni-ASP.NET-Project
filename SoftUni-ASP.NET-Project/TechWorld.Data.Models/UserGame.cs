using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TechWorld.Data.Models
{
    [PrimaryKey(nameof(UserId), nameof(GameId))]
    public class UserGame
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;
        public virtual IdentityUser User { get; set; } = null!;

        [ForeignKey(nameof(Game))]
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; } = null!;
    }
}
