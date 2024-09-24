using ASPTM.MIddleware;
using ASPTM.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ASPTM.Helpers
{
    public class FlatsContractHelper
    {
        private FlatsContractMiddleware fc_middleware;
        private FlatMiddleware f_middleware;

        public FlatsContractHelper(FlatsContractMiddleware fc_middleware, FlatMiddleware f_middleware)
        {
            this.fc_middleware = fc_middleware;
            this.f_middleware = f_middleware;
        }
        //public async Task<IList<(FlatsContract, Flat)>> GetFlatsContractsAndFlats()
        //{
        //    IEnumerable<FlatsContract> flatscs = fc_middleware.GetFlatsContracts();
        //    IList<(FlatsContract, Flat)> flatscnF = new List<(FlatsContract, Flat)>();
        //    foreach (var item in flatscs)
        //    {
        //        var flat = await f_middleware.GetFlat(item.Fid);
        //        (FlatsContract, Flat) t1 = (item, flat);
        //        flatscnF.Add(t1);
        //    }
        //    return flatscnF;
        //}
        
    }
}
