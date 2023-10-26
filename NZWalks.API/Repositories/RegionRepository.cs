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

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await context.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (region == null)
            {
                return null;
            }
            
            //Delete the region

            context.Regions.Remove(region);
            await context.SaveChangesAsync();
            return region;
        }

        public async Task<Region> updateAsync(Guid id, Region region)
        {
            var exisitngRegion = await context.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (exisitngRegion == null)
            {
                return null;
            }

            exisitngRegion.Code = region.Code;
            exisitngRegion.Walk = region.Walk;
            exisitngRegion.Name = region.Name;
            exisitngRegion.Lat = region.Lat;
            exisitngRegion.Long = region.Long;
            exisitngRegion.Area = region.Area;

            await context.SaveChangesAsync();
            return exisitngRegion;
        }
    }
}
