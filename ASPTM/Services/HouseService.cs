using ASPTM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ASPTM.Services
{
    public class HouseService
    {
        private RealestaterentalContext db;
        private IMemoryCache cache;
        private readonly int cacheDuration = 252;
        private string HouseChacheKey = "Houses";

        public HouseService(RealestaterentalContext db, IMemoryCache cache)
        {
            this.db = db;
            this.cache = cache;
        }

        public async Task<IEnumerable<House>> GetHouses()
        {
            IEnumerable<House> houses = null;
            if (!cache.TryGetValue(HouseChacheKey, out houses))
            {
                houses = await db.Houses.ToListAsync();
                if (houses != null)
                {
                    cache.Set(HouseChacheKey, houses, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheDuration)));
                }
            }
            return houses;
        }

        public async Task<House> GetFlat(int id)
        {
            House house = null;
            if (!cache.TryGetValue(id, out house))
            {
                house = await db.Houses.FirstOrDefaultAsync(l => l.Pid == id);
                if (house != null)
                {
                    cache.Set(house.Pid, house,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheDuration)));
                }
            }

            return house;
        }

        public async Task AddFlat(House house)
        {
            db.Houses.Add(house);
            int n = await db.SaveChangesAsync();
            if (n > 0)
            {
                cache.Set(house.Pid, house, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheDuration)
                });
            }
        }
    }
}
