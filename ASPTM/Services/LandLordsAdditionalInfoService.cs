using ASPTM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ASPTM.Services
{
    public class LandLordsAdditionalInfoService(RealestaterentalContext db, IMemoryCache cache)
    {
        private readonly Int32 _cacheDuration = 300;
        private readonly String _landLordsChacheKey = "LandLordsAdditionalInfo_All";

        public async Task<IEnumerable<LandLordsAdditionalInfo>> GetLandLordsAdditionalInfo(int pagenumber, int pagesize)
        {
            int maxPages = (int)db.LandLordsAdditionalInfos.Count() % pagesize == 0 ? 
                (int)db.LandLordsAdditionalInfos.Count() / pagesize 
                : (int)(db.LandLordsAdditionalInfos.Count() / pagesize) + 1;
            if (pagenumber < 1 || pagenumber > maxPages)
            {
                pagenumber = 1;
            }
            IEnumerable<LandLordsAdditionalInfo> landLordsAdditional = null;
            if (!cache.TryGetValue(_landLordsChacheKey + $"_{pagenumber}", out landLordsAdditional))
            {
                landLordsAdditional = await db.LandLordsAdditionalInfos.Skip(pagesize * (pagenumber - 1))
                    .Take(pagesize)
                    .ToListAsync();
                if (landLordsAdditional != null)
                {
                    cache.Set(_landLordsChacheKey + $"_{pagenumber}", landLordsAdditional,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheDuration)));
                }
            }
            return landLordsAdditional;
        }

        public async Task<LandLordsAdditionalInfo> GetLandLordAdditionalInfo(Int32 Llid)
        {
            LandLordsAdditionalInfo landLordsAdditional = null;
            if (!cache.TryGetValue("LandLordsAdditionalInfo" + Llid, out landLordsAdditional))
            {
                landLordsAdditional = await db.LandLordsAdditionalInfos.FirstOrDefaultAsync(l => l.Llid == Llid);
                if (landLordsAdditional != null)
                {
                    cache.Set("LandLordsAdditionalInfo" + landLordsAdditional.Llid, landLordsAdditional,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheDuration)));
                }
            }

            return landLordsAdditional;
        }

        public async Task AddLandLordsAdditionalInfo(LandLordsAdditionalInfo landLordsAdditional)
        {
            db.LandLordsAdditionalInfos.Add(landLordsAdditional);
            Int32 n = await db.SaveChangesAsync();
            if (n > 0)
            {
                cache.Set("LandLordsAdditionalInfo" + landLordsAdditional.Llid, landLordsAdditional, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheDuration)
                });
            }
        }

        public async Task<Int32> UpdateLandLordsAdditionalInfo(LandLordsAdditionalInfo landLordsAdditional)
        {
            if (!db.LandLordsAdditionalInfos.Any(l => l.Llid == landLordsAdditional.Llid))
            {
                return 0;
            }

            db.LandLordsAdditionalInfos.Update(landLordsAdditional);
            Int32 n = await db.SaveChangesAsync();

            if (n > 0)
            {
                cache.Set("LandLordsAdditionalInfo" + landLordsAdditional.Llid, landLordsAdditional, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_cacheDuration)
                });
            }

            return n;
        }

        public async Task<Int32> DeleteLandLordAdditionalInfo(Int32 Llid)
        {
            LandLordsAdditionalInfo landLordsAdditional = null;
            landLordsAdditional = await db.LandLordsAdditionalInfos.FirstOrDefaultAsync(l => l.Llid == Llid);
            if (landLordsAdditional == null)
            {
                return 0;
            }
            db.LandLordsAdditionalInfos.Remove(landLordsAdditional);
            Int32 n = await db.SaveChangesAsync();
            return n;
        }
    }
}
