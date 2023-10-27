using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalksDifficultyRepository
    {
        Task<IEnumerable<WalkDifficulty>> GetAllWalksAsync();
        Task<WalkDifficulty> GetAsync(Guid id);
        Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty);
        Task<WalkDifficulty> DeleteAsync(Guid id);
        Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty);
    }
}
