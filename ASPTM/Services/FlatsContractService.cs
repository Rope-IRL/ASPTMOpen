using ASPTM.Models;
using ASPTM.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace ASPTM.Services
{
    public class FlatsContractService
    {
        private RealestaterentalContext db;
        private IMemoryCache cache;
        private readonly int cacheDuration = 252;
        private string LesseeChacheKey = "FlatsContract_All";
        private string FlatChacheKey = "ContractsAndFlats";

        public FlatsContractService(RealestaterentalContext db, IMemoryCache cache)
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
                flatsContracts = await db.FlatsContracts.Take(20).ToListAsync();
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
                    cache.Set(FlatChacheKey, fcf, new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheDuration)));
                }

            }
            return fcf;
        }

        public async Task<IEnumerable<FlatsContractInfo>> GetFlatsContractsFullInfoAsync(int pagenumber, int pagesize)
        {
            int maxPages = (int)db.FlatsContracts.Count() % pagesize == 0 ? (int)db.FlatsContracts.Count() / pagesize : (int)(db.FlatsContracts.Count() / pagesize) + 1;
            if (pagenumber < 1 || pagenumber > maxPages)
            {
                pagenumber = 1;
            }

            IEnumerable<FlatsContractInfo> flatsContractsInfo = null;
            if (!cache.TryGetValue(FlatChacheKey, out flatsContractsInfo))
            {
                IEnumerable<FlatsContract> flatsContracts = await db.FlatsContracts.Include(flat => flat.FidNavigation)
                    .Include(landLord => landLord.Ll)
                    .ThenInclude(llAdditionalInfo => llAdditionalInfo.LandLordsAdditionalInfo)
                    .Include(lessee => lessee.LidNavigation)
                    .ThenInclude(lAdditionalInfo => lAdditionalInfo.LesseesAdditionalInfo)
                    .Skip(pagesize * (pagenumber - 1))
                    .OrderBy(flat => flat.Id)
                    .Take(pagesize).ToListAsync();

                flatsContractsInfo = flatsContracts.Select(flatsContract => new FlatsContractInfo
                {
                    Id = flatsContract.Id,
                    StartDate = flatsContract.StartDate,
                    EndDate = flatsContract.EndDate,
                    Cost = flatsContract.Cost,
                    FlatId = flatsContract.Fid,
                    FlatAddress = flatsContract.FidNavigation.Address,
                    LesseeName = flatsContract.LidNavigation.LesseesAdditionalInfo.Name,
                    LesseeSurName = flatsContract.LidNavigation.LesseesAdditionalInfo.Surname,
                    LandLordName = flatsContract.Ll.LandLordsAdditionalInfo.Name,
                    LandLordSurName = flatsContract.Ll.LandLordsAdditionalInfo.Surname

                });

                if (flatsContractsInfo != null)
                {
                    cache.Set(FlatChacheKey, flatsContractsInfo, new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheDuration)));
                }

            }
            return flatsContractsInfo;
        }

        public async Task<FlatsContract> GetFlatsContract(int id)
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
            int n = await db.SaveChangesAsync();
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
