using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;

namespace NZWalks.API.Repositories
{
    
    public class SQLRegionRepository : IRegionRepository
    {
        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public NZWalksDbContext DbContext { get; }

        public async Task<Region> CreateAsync(Region region)
        {
          await DbContext.Regions.AddAsync(region);
          await DbContext.SaveChangesAsync();
            return region; 
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await DbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await DbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await DbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null) { 
                return null;
            }

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await DbContext.SaveChangesAsync();
            return existingRegion;

        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
          var existingRegion=  await DbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
            {
                return null;
            }
            DbContext.Regions.Remove(existingRegion);
            await DbContext.SaveChangesAsync();
            return existingRegion;
        }

    }
}
