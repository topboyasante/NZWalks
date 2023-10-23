using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> getAllRegionsAsync();
        Task<Region> GetAsync(Guid id);
        Task<Region> AddAsync(Region region);
    }
}
