using TechWorld.Web.ViewModels;

namespace TechWorld.Services.Core.Interfaces
{
    public interface ICartService
    {
        Task AddAsync(Guid userId, Guid gameId);
        Task RemoveAsync(Guid userId, Guid gameId);
        Task<IEnumerable<CartViewModel?>> GetUserCartAsync(Guid userId);
        Task ClearCartAsync(Guid userId);
    }
}
