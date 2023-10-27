using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDBContext context;

        public WalkRepository(NZWalksDBContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Walk>> GetAllWalksAsync()
        {
            return await context.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            //Find the walk
            var walk = await context.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (walk == null)
            {
                return null;
            }

            //Delete the region
            context.Walks.Remove(walk);
            await context.SaveChangesAsync();
            return walk;
        }

        public Task<Walk> GetAsync(Guid id)
        {
            return context.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id ==  id);
        }
        public async Task<Walk> AddAsync(Walk walk)
        {
            //Assign a new ID
            walk.Id = Guid.NewGuid();
            //Add the walk to the DB
            await context.Walks.AddAsync(walk);
            //Save the changes to the DB
            await context.SaveChangesAsync();
            //Return the new walk you added
            return walk;
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await context.Walks.FindAsync(id);

            if(existingWalk == null)
            {
                return null;
            }

            existingWalk.Length = walk.Length;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.Name = walk.Name;
            existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
            context.SaveChangesAsync();

            return existingWalk;
        }
    }
}
