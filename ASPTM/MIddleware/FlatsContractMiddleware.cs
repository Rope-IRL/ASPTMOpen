using ASPTM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace ASPTM.MIddleware
{
    public class FlatsContractMiddleware
    {
        private RealestaterentalContext db;
        private IMemoryCache cache;
        private readonly Int32 cacheDuration = 252;
        private String LesseeChacheKey = "FlatsContract_All";
        private String FlatChacheKey = "ContractsAndFlats";

        public FlatsContractMiddleware(RealestaterentalContext db, IMemoryCache cache)
        {
            this.db = db;
            this.cache = cache;
        }

        public async Task<IList<(FlatsContract, Flat)>> GetFlatsContractsAsync()
        {
            IEnumerable<FlatsContract> flatsContracts = null;
            IList<(FlatsContract, Flat)> fcf = new List<(FlatsContract, Flat)>();
            if (!cache.TryGetValue(FlatChacheKey, out flatsContracts))
            {
                flatsContracts =  await db.FlatsContracts.Take(20).ToListAsync();
                foreach (var item in flatsContracts)
                {
                    if (item != null)
                    {
                        var flat = db.Flats.Find(item.Fid);
                        if (flat != null)
                        {
                            fcf.Add((item, flat));
                        }
                    }
                }
                if (fcf != null)
                {
                    cache.Set(FlatChacheKey, fcf, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheDuration)));
                }

            }
            return fcf;
        }

        public async Task<FlatsContract> GetFlatsContract(Int32 id)
        {
            FlatsContract flatsContract = null;
            if (!cache.TryGetValue(id, out flatsContract))
            {
                flatsContract = await db.FlatsContracts.FirstOrDefaultAsync(l => l.Fid == id);
                if (flatsContract != null)
                {
                    cache.Set(flatsContract.Fid, flatsContract,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheDuration)));
                }
            }

            return flatsContract;
        }

        public async Task AddFlatsContract(FlatsContract flatsContract)
        {
            db.FlatsContracts.Add(flatsContract);
            Int32 n = await db.SaveChangesAsync();
            if (n > 0)
            {
                cache.Set(flatsContract.Fid, flatsContract, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheDuration)
                });
            }
        }
    }
}
