using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechWorld.Data.Models;
using TechWorld.Web.ViewModels;

namespace TechWorld.Services.Core.Interfaces
{
    public interface IWishlistService
    {
        Task AddAsync(Guid userId, Guid gameId);
        Task RemoveAsync(Guid userId, Guid gameId);
        Task<IEnumerable<WishlistViewModel?>> GetUserWishlistByIdAsync(Guid userId);
        Task<bool> ExistsAsync(Guid userId, Guid gameId);
    }
}
