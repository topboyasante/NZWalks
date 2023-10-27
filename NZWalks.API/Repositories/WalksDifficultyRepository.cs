using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalksDifficultyRepository:IWalksDifficultyRepository
    {
        private readonly NZWalksDBContext context;

        public WalksDifficultyRepository(NZWalksDBContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<WalkDifficulty>> GetAllWalksAsync()
        {
            return await context.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            //Assign a new ID
            walkDifficulty.Id = Guid.NewGuid();
            //Add the walkDifficulty to the DB
            await context.WalkDifficulty.AddAsync(walkDifficulty);
            //Save the changes to the DB
            await context.SaveChangesAsync();
            //Return the new walkDifficulty you added
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            //Find the walk
            var walkDifficulty = await context.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);

            if (walkDifficulty == null)
            {
                return null;
            }

            //Delete the region
            context.WalkDifficulty.Remove(walkDifficulty);
            await context.SaveChangesAsync();
            return walkDifficulty;
        }

        public Task<WalkDifficulty> GetAsync(Guid id)
        {
            return context.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await context.WalkDifficulty.FindAsync(id);

            if (existingWalkDifficulty == null)
            {
                return null;
            }

            existingWalkDifficulty.Code = walkDifficulty.Code;
            context.SaveChangesAsync();

            return existingWalkDifficulty;
        }
    }
}
