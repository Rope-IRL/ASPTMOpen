using ASPTM.Models;
using ASPTM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPTM.Controllers
{
    public class FlatsContractController : Controller
    {
        private FlatsContractService fc_middleware;


        public FlatsContractController( FlatsContractService fc_middleware)
        {
            this.fc_middleware = fc_middleware;
        }
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Location =ResponseCacheLocation.Any, Duration =252)]
        public async Task<IActionResult> Table()
        {

            var flatscs = await fc_middleware.GetFlatsContractsFullInfoAsync(1, 20);
            if (flatscs != null) { 
                return View(flatscs);
            }
            return View();
        }

    }
}
