using ASPTM.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPTM.Helpers
{
    public class FlatHelper
    {
        private RealestaterentalContext db = new RealestaterentalContext();
        public async Task<IEnumerable<Flat>> GetFlats()
        {
            IEnumerable<Flat> flats = await db.Flats.ToListAsync();
            return flats;
        }

        public  Flat GetFlat(Int32 id)
        {
            Flat flat = db.Flats.FirstOrDefault(l => l.Fid == id);
            return flat;
        }
    }
}
