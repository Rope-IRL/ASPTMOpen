using ASPTM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ASPTM.MIddleware
{
    public class HouseMiddleware
    {
        private RealestaterentalContext db;
        private IMemoryCache cache;
        private readonly Int32 cacheDuration = 252;
        private String HouseChacheKey = "Houses";

        public HouseMiddleware(RealestaterentalContext db, IMemoryCache cache)
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

        public async Task<House> GetFlat(Int32 id)
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
            Int32 n = await db.SaveChangesAsync();
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
