using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechWorld.Data.Models
{
    [PrimaryKey(nameof(UserId), nameof(GameId))]
    public class UserGame
    {
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;

        [ForeignKey(nameof(Game))]
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; } = null!;
    }
}
