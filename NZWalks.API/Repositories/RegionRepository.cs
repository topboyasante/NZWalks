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

        public IEnumerable<Region> getAllRegions()
        {
            return context.Regions.ToList();
        }
    }
}
