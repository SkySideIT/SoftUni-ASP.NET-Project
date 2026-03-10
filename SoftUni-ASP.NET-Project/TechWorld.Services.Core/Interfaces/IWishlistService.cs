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
        Task AddAsync(string userId, Guid gameId);
        Task RemoveAsync(string userId, Guid gameId);
        Task<IEnumerable<WishlistViewModel?>> GetUserWishlistByIdAsync(string userId);
        Task<bool> ExistsAsync(string userId, Guid gameId);
    }
}
