using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDBContext context;

        public RegionRepository(NZWalksDBContext context) {
            this.context = context;
        }

        public async Task<IEnumerable<Region>> getAllRegionsAsync()
        {
            return await context.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
           return await context.Regions.FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await context.Regions.AddAsync(region);
            await context.SaveChangesAsync();
            return region;
        }

    }
}
